// --------------------------------------------------------------------------------------------------
// <copyright file = "MessageIngestor.cs" company="GrabCaster Ltd">
//   Copyright (c) 2013 - 2016 GrabCaster Ltd All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
// 
//    Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------

using GrabCaster.Framework.Contracts.Storage;

namespace GrabCaster.Framework.Engine.OnRamp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.Serialization.Object;
    using GrabCaster.Framework.Storage;
    using Newtonsoft.Json;

    /// <summary>
    /// Engine main message ingestor
    /// </summary>
    public static class MessageIngestor
    {
        public delegate void SetConsoleActionEventEmbedded(string DestinationConsolePointId, IBubblingObject bubblingObject);
        /// <summary>
        /// Used internally by the embedded
        /// </summary>
        public static SetConsoleActionEventEmbedded setConsoleActionEventEmbedded { get; set; }


        private static bool secondaryPersistProviderEnabled;
        private static int secondaryPersistProviderByteSize;
        private static IDevicePersistentProvider DevicePersistentProvider;
        private static readonly object[] ParametersPersistEventFromBlob = { null };

        public static void InitSecondaryPersistProvider()
        {
            try
            {

                secondaryPersistProviderEnabled = ConfigurationBag.Configuration.SecondaryPersistProviderEnabled;
                secondaryPersistProviderByteSize = ConfigurationBag.Configuration.SecondaryPersistProviderByteSize;

                // Load the abrstracte persistent provider
                var devicePersistentProviderComponent = Path.Combine(
                                    ConfigurationBag.Configuration.DirectoryOperativeRootExeName,
                                    ConfigurationBag.Configuration.PersistentProviderComponent);

                // Create the reflection method cached 
                var assemblyPersist = Assembly.LoadFrom(devicePersistentProviderComponent);

                // Main class logging
                var assemblyClassDpp = (from t in assemblyPersist.GetTypes()
                                        let attributes = t.GetCustomAttributes(typeof(DevicePersistentProviderContract), true)
                                        where t.IsClass && attributes != null && attributes.Length > 0
                                        select t).First();


                DevicePersistentProvider = Activator.CreateInstance(assemblyClassDpp) as IDevicePersistentProvider;

            }
            catch (Exception ex)
            {

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    Constant.LogLevelError);
            }
        }
        public static void IngestMessagge(BubblingObject bubblingObject)
        {


       
            try
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                                    $"IngestMessagge bubblingObject.SenderChannelId {bubblingObject.SenderChannelId } " +
                                    $"bubblingObject.SenderPointId {bubblingObject.SenderPointId} " +
                                    $"bubblingObject.DestinationChannelId {bubblingObject.DestinationChannelId} " +
                                    $" bubblingObject.DestinationPointId {bubblingObject.DestinationPointId} " +
                                    $"bubblingObject.MessageType {bubblingObject.MessageType}" +
                                    $"bubblingObject.Persisting {bubblingObject.Persisting} " +
                                    $"bubblingObject.MessageId {bubblingObject.MessageId} " +
                                    $"bubblingObject.Name {bubblingObject.Name}",
                                    Constant.LogLevelError,
                                    Constant.TaskCategoriesConsole,
                                    null,
                                    Constant.LogLevelVerbose);
                //Check if message is for this point
                var receiverChannelId = bubblingObject.DestinationChannelId;
                var receiverPointId = bubblingObject.DestinationPointId;

                var requestAvailable = (receiverChannelId == ConfigurationBag.Configuration.ChannelId
                                        && receiverPointId == ConfigurationBag.Configuration.PointId)
                                       || (receiverChannelId == ConfigurationBag.ChannelAll
                                           && receiverPointId == ConfigurationBag.Configuration.PointId)
                                       || (receiverChannelId == ConfigurationBag.Configuration.ChannelId
                                           && receiverPointId == ConfigurationBag.PointAll)
                                       || (receiverChannelId == ConfigurationBag.ChannelAll
                                           && receiverPointId == ConfigurationBag.PointAll);

                if (!requestAvailable)
                {
                    // ****************************NOT FOR ME*************************
                    return;
                }

                if (bubblingObject.SenderPointId == ConfigurationBag.Configuration.PointId)
                {
                    // **************************** HA AREA *************************


                    if (bubblingObject.MessageType == "HA" && bubblingObject.HAGroup == ConfigurationBag.Configuration.HAGroup &&
                            EventsEngine.HAEnabled)
                    {

                        //If HA group member and HA 

                        EventsEngine.HAPoints[EventsEngine.HAPointTickId] = DateTime.Now;
                        long haTickFrom = long.Parse(UTF8Encoding.UTF8.GetString(bubblingObject.Data));

                        //if same tick then return because same point
                        if (haTickFrom == EventsEngine.HAPointTickId)
                            return;

                        DateTime dt;
                        lock (EventsEngine.HAPoints)
                        {
                            //If not exist then add
                            if (!EventsEngine.HAPoints.TryGetValue(haTickFrom, out dt))
                                EventsEngine.HAPoints.Add(haTickFrom, DateTime.Now);
                            else
                            {
                                EventsEngine.HAPoints[haTickFrom] = DateTime.Now;
                            }
                        }

                        byte[] content = UTF8Encoding.UTF8.GetBytes(EventsEngine.HAPointTickId.ToString());

                        BubblingObject bubblingObjectToSync = new BubblingObject(content);

                        bubblingObject.MessageType = "HA";
                        OffRampEngineSending.SendMessageOffRamp(bubblingObjectToSync,
                                                                "HA",
                                                                bubblingObject.SenderChannelId,
                                                                bubblingObject.SenderPointId,
                                                                string.Empty);




                    }
                    else
                    {
                        return;
                    }
                }
                // ****************************GET FROM STORAGE IF REQUIRED*************************
                if (bubblingObject.Persisting)
                {
                    bubblingObject = (BubblingObject)SerializationEngine.ByteArrayToObject(DevicePersistentProvider.PersistEventFromStorage(bubblingObject.MessageId));
                }


                #region EVENT
                // ****************************IF EVENT TYPE*************************
                if (bubblingObject.MessageType == "Event")
                {
                    //If HA group member and HA 
                    if (EventsEngine.HAEnabled)
                    {
                        //Check if is the first in list, if not then discard
                        EventsEngine.HAPoints.OrderBy(key => key.Key);
                        if (EventsEngine.HAPoints.Count > 1 &&
                            EventsEngine.HAPoints.First().Key != EventsEngine.HAPointTickId)
                        {
                            return;
                        }
                    }
                        //Check if is Syncronouse response
                    if (bubblingObject.SyncronousFromEvent &&
                    bubblingObject.SenderPointId == ConfigurationBag.Configuration.PointId)
                    {
                        //Yes it's a syncronous response from my request from this pointid
                        //Execute the delegate and exit
                        var propDataContext = bubblingObject.DataContext;
                        bubblingObject.SyncronousFromEvent = false;
                        EventsEngine.SyncAsyncEventsExecuteDelegate(bubblingObject.SyncronousToken,(byte[]) propDataContext);
                        bubblingObject.SenderPointId = "";
                        bubblingObject.SyncronousToken = "";
                        return;
                    }

                    // ****************************EVENT EXIST EXECUTE*************************
                    EventsEngine.ExecuteEventsInTrigger(
                        bubblingObject,
                        bubblingObject.Events[0],
                        false,
                        bubblingObject.SenderPointId);
                    return;
                }

                #endregion


                #region CONSOLE



                // **************************** SYNC AREA *************************

                //******************* OPERATION CONF BAG- ALL THE CONF FILES AND DLLS ****************************************************************
                //Receive a package folder to syncronize him self
                if (bubblingObject.MessageType == "SyncPull")
                {
                    byte[] content = GrabCaster.Framework.CompressionLibrary.Helpers.CreateFromDirectory(ConfigurationBag.Configuration.DirectoryOperativeRootExeName);

                    BubblingObject bubblingObjectToSync = new BubblingObject(content);
                    bubblingObject.MessageType = "SyncPush";
                    OffRampEngineSending.SendMessageOffRamp(bubblingObjectToSync,
                                                            "SyncPush",
                                                            bubblingObject.SenderChannelId,
                                                            bubblingObject.SenderPointId,
                                                            string.Empty);




                }
                //Receive the request to send the bubbling
                if (bubblingObject.MessageType == "SyncPush")
                {
                    LogEngine.DirectEventViewerLog($"Received a syncronization package from channel ID {bubblingObject.SenderChannelId} and point ID {bubblingObject.SenderChannelId}\rAutoSyncronizationEnabled parameter = {ConfigurationBag.Configuration.AutoSyncronizationEnabled}",2);
                    if (ConfigurationBag.Configuration.AutoSyncronizationEnabled)
                    {
                        byte[] bubblingContent = SerializationEngine.ObjectToByteArray(bubblingObject.Data);
                        string currentSyncFolder = ConfigurationBag.SyncDirectorySyncIn();
                        GrabCaster.Framework.CompressionLibrary.Helpers.CreateFromBytearray(bubblingObject.Data, currentSyncFolder);
                    }

                }

                #endregion

            }
            catch (Exception ex)
            {


                LogEngine.WriteLog(ConfigurationBag.EngineName,
                                  $"Error in {MethodBase.GetCurrentMethod().Name}",
                                  Constant.LogLevelError,
                                  Constant.TaskCategoriesError,
                                  ex,
                                  Constant.LogLevelError);




            }
        }
    }
}