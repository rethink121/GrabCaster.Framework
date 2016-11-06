// MessageIngestor.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.CompressionLibrary;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Storage;
using GrabCaster.Framework.Engine.OffRamp;
using GrabCaster.Framework.Log;
using GrabCaster.Framework.Serialization.Object;

#endregion

namespace GrabCaster.Framework.Engine.OnRamp
{
    /// <summary>
    ///     Engine main message ingestor
    /// </summary>
    public static class MessageIngestor
    {
        public delegate void SetConsoleActionEventEmbedded(
            string DestinationConsolePointId, IBubblingObject bubblingObject);


        private static bool secondaryPersistProviderEnabled;
        private static int secondaryPersistProviderByteSize;
        private static IDevicePersistentProvider DevicePersistentProvider;
        private static readonly object[] ParametersPersistEventFromBlob = {null};

        /// <summary>
        ///     Used internally by the embedded
        /// </summary>
        public static SetConsoleActionEventEmbedded setConsoleActionEventEmbedded { get; set; }

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
                    $"IngestMessagge bubblingObject.SenderChannelId {bubblingObject.SenderChannelId} " +
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

                //If local event then execute
                if (bubblingObject.LocalEvent)
                {
                    //new Task(() =>
                    //{
                    //    EventsEngine.ExecuteEventsInTrigger(
                    //    bubblingObject,
                    //    bubblingObject.Events[0],
                    //    false,
                    //    bubblingObject.SenderPointId);

                    //}).Start();
                    EventsEngine.ExecuteEventsInTrigger(
                        bubblingObject,
                        bubblingObject.Events[0],
                        false,
                        bubblingObject.SenderPointId);
                    return;
                }

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


                    if (bubblingObject.MessageType == "HA" &&
                        bubblingObject.HAGroup == ConfigurationBag.Configuration.HAGroup &&
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
                    bubblingObject =
                        (BubblingObject)
                        SerializationEngine.ByteArrayToObject(
                            DevicePersistentProvider.PersistEventFromStorage(bubblingObject.MessageId));
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
                        EventsEngine.SyncAsyncEventsExecuteDelegate(bubblingObject.SyncronousToken, propDataContext);
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
                    byte[] content =
                        Helpers.CreateFromDirectory(
                            ConfigurationBag.Configuration.DirectoryOperativeRootExeName);

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
                    LogEngine.DirectEventViewerLog(
                        $"Received a syncronization package from channel ID {bubblingObject.SenderChannelId} and point ID {bubblingObject.SenderChannelId}\rAutoSyncronizationEnabled parameter = {ConfigurationBag.Configuration.AutoSyncronizationEnabled}",
                        2);
                    if (ConfigurationBag.Configuration.AutoSyncronizationEnabled)
                    {
                        byte[] bubblingContent = SerializationEngine.ObjectToByteArray(bubblingObject.Data);
                        string currentSyncFolder = ConfigurationBag.SyncDirectorySyncIn();
                        Helpers.CreateFromBytearray(bubblingObject.Data, currentSyncFolder);
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