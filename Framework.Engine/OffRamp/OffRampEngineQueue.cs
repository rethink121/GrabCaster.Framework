// --------------------------------------------------------------------------------------------------
// <copyright file = "OffRampEngine.cs" company="GrabCaster Ltd">
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

//using GrabCaster.Framework.PerfCounters;

using GrabCaster.Framework.Contracts.Storage;

namespace GrabCaster.Framework.Engine.OffRamp
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
    using GrabCaster.Framework.Engine.OnRamp;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.Serialization.Object;
    using Microsoft.ServiceBus.Messaging;

    using Newtonsoft.Json;

    /// <summary>
    /// Internal messaging Queue
    /// </summary>
    public sealed class OffRampEngineQueue : LockSlimQueueEngine<BubblingObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OffRampEngineQueue"/> class.
        /// </summary>
        /// <param name="capLimit">
        /// TODO The cap limit.
        /// </param>
        /// <param name="timeLimit">
        /// TODO The time limit.
        /// </param>
        public OffRampEngineQueue(int capLimit, int timeLimit)
        {
            this.CapLimit = capLimit;
            this.TimeLimit = timeLimit;
            this.InitTimer();
        }
    }

    /// <summary>
    /// Last line of receiving and first before the message ingestor
    /// </summary>
    public static class OffRampEngineSending
    {
        //Performance counter

        /// <summary>
        /// The off ramp engine.
        /// </summary>
        private static OffRampEngineQueue OffRampEngineQueue;

        /// <summary>
        /// Off Ramp Component
        /// </summary>
        private static IOffRampStream OffRampStream;

        private static IDevicePersistentProvider DevicePersistentProvider;

        private static bool secondaryPersistProviderEnabled;

        private static int secondaryPersistProviderByteSize;
        
        /// <summary>
        /// Initialize the onramp engine the OffRampPatternComponent variable is for the next version
        /// </summary>
        /// <param name="offRampPatternComponent">
        /// The Off Ramp Pattern Component.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Init(string offRampPatternComponent)
        {
            try
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    $"Start engine initialization.",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    null,
                    Constant.LogLevelWarning);


                if (ConfigurationBag.Configuration.RunLocalOnly)
                {

                    LogEngine.WriteLog(ConfigurationBag.EngineName,
                                        $"OffRamp provider not started, this GrabCaster point is configured for local execution only.",
                                        Constant.LogLevelError,
                                        Constant.TaskCategoriesError,
                                        null,
                                        Constant.LogLevelWarning);
                    return false;
                }
                Debug.WriteLine("Initialize Abstract Event Up Stream Engine.");

                // Load event up stream external component
                var eventsUpStreamComponent = Path.Combine(
                    ConfigurationBag.Configuration.DirectoryOperativeRootExeName,
                    ConfigurationBag.Configuration.EventsStreamComponent);

                // Create the reflection method cached 
                var assembly = Assembly.LoadFrom(eventsUpStreamComponent);
                
                // Main class logging
                var assemblyClass = (from t in assembly.GetTypes()
                                     let attributes = t.GetCustomAttributes(typeof(EventsOffRampContract), true)
                                     where t.IsClass && attributes != null && attributes.Length > 0
                                     select t).First();

                OffRampStream = Activator.CreateInstance(assemblyClass) as IOffRampStream;
 
                OffRampEngineQueue = new OffRampEngineQueue(
                    ConfigurationBag.Configuration.ThrottlingOffRampIncomingRateNumber, 
                    ConfigurationBag.Configuration.ThrottlingOffRampIncomingRateSeconds);
                OffRampEngineQueue.OnPublish += OffRampEngineQueueOnPublish;

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    "Start Off Ramp Engine.", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    null, 
                    Constant.LogLevelInformation);

                // Inizialize the Dpp
                Debug.WriteLine("Initialize Abstract Storage Provider Engine.");
                //todo optimization utilizzare linterfaccia
                OffRampStream.CreateOffRampStream();
                secondaryPersistProviderEnabled= ConfigurationBag.Configuration.SecondaryPersistProviderEnabled;
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

                return true;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    Constant.LogLevelError);
                return false;
            }
        }

        /// <summary>
        /// Queue the message directly into the spool queue
        /// </summary>
        /// <param name="bubblingObject"></param>
        public static void QueueMessage(BubblingObject bubblingObject)
        {
            OffRampEngineQueue.Enqueue(bubblingObject);
        }

        /// <summary>
        /// TODO The send message on ramp.
        /// </summary>
        /// <param name="bubblingTriggerConfiguration">
        /// TODO The bubbling trigger configuration.
        /// </param>
        /// <param name="ehMessageType">
        /// TODO The eh message type.
        /// </param>
        /// <param name="channelId">
        /// TODO The channel id.
        /// </param>
        /// <param name="pointId">
        /// TODO The point id.
        /// </param>
        /// <param name="properties">
        /// TODO The properties.
        /// </param>
        public static void SendMessageOffRamp(
            BubblingObject bubblingObject, 
            string messageType, 
            string channelId, 
            string pointId, 
            string pointIdOverrided)
        {
            try
            {

                //Create message Id
                bubblingObject.MessageId = Guid.NewGuid().ToString();
                
                //Create bubbling object message to send
                byte[] serializedMessage = SerializationEngine.ObjectToByteArray(bubblingObject);

                //If enabled and > secondaryPersistProviderByteSize then store the body
                //put the messageid in the body message to recover
                bubblingObject.Persisting = serializedMessage.Length > secondaryPersistProviderByteSize &&
                                            secondaryPersistProviderEnabled;

                if (bubblingObject.Persisting)
                {
                    bubblingObject.Data = EncodingDecoding.EncodingString2Bytes(bubblingObject.MessageId);
                    DevicePersistentProvider.PersistEventToStorage(serializedMessage, bubblingObject.MessageId);
                    bubblingObject.Persisting = true;
                }
                // Message context
                bubblingObject.MessageType = messageType;

                if(pointIdOverrided != null)
                    bubblingObject.SenderPointId = pointIdOverrided;
                else
                    bubblingObject.SenderPointId = ConfigurationBag.Configuration.PointId;

                bubblingObject.SenderName = ConfigurationBag.Configuration.PointName;
                bubblingObject.SenderDescriprion= ConfigurationBag.Configuration.PointDescription;
                bubblingObject.SenderChannelId = ConfigurationBag.Configuration.ChannelId;
                bubblingObject.SenderChannelName = ConfigurationBag.Configuration.ChannelName;
                bubblingObject.SenderChannelDescription = ConfigurationBag.Configuration.ChannelDescription;
                bubblingObject.SenderPointId= ConfigurationBag.Configuration.PointId;
                bubblingObject.SenderName= ConfigurationBag.Configuration.PointName;
                bubblingObject.SenderDescriprion= ConfigurationBag.Configuration.PointDescription;
                bubblingObject.DestinationChannelId= channelId;
                bubblingObject.DestinationPointId= pointId;
                bubblingObject.HAGroup = ConfigurationBag.Configuration.HAGroup;

                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    $"SendMessageOffRamp bubblingObject.SenderChannelId {bubblingObject.SenderChannelId } " +
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

                OffRampEngineQueue.Enqueue(bubblingObject);
       
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

        public static void SendNullMessageOffRamp(
            string messageType, 
            string channelId, 
            string pointId, 
            string idComponent, 
            string subscriberId,
            string pointIdOverrided)
        {
            try
            {

                var bubblingObject = new BubblingObject(EncodingDecoding.EncodingString2Bytes(string.Empty));
                bubblingObject.Persisting = false;

                bubblingObject.MessageId = Guid.NewGuid().ToString();
                bubblingObject.SubscriberId = subscriberId;
                bubblingObject.MessageType = messageType;

                string senderid;

                if(pointIdOverrided != null)
                    senderid= pointIdOverrided;
                else
                    senderid = ConfigurationBag.Configuration.PointId;

                bubblingObject.SenderPointId = senderid;
                bubblingObject.SenderName = ConfigurationBag.Configuration.PointName;
                bubblingObject.SenderDescriprion = ConfigurationBag.Configuration.PointDescription;
                bubblingObject.SenderChannelId= ConfigurationBag.Configuration.ChannelId;
                bubblingObject.SenderChannelName = ConfigurationBag.Configuration.ChannelName;
                bubblingObject.SenderChannelDescription = ConfigurationBag.Configuration.ChannelDescription;
                bubblingObject.DestinationChannelId = channelId;
                bubblingObject.DestinationPointId= pointId;
                bubblingObject.IdComponent= idComponent;

                LogEngine.WriteLog(ConfigurationBag.EngineName,
                            $"SendNullMessageOffRamp bubblingObject.SenderChannelId {bubblingObject.SenderChannelId } " +
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
                            Constant.LogLevelInformation);

                // Queue the data
                OffRampEngineQueue.Enqueue(bubblingObject);

                Debug.WriteLine(
                    $"Sent Message Type: {messageType} - To ChannelID: {channelId} PointID: {pointId}");
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


        /// <summary>
        /// TODO The off ramp engine on publish.
        /// </summary>
        /// <param name="bubblingObjects"></param>
        private static void OffRampEngineQueueOnPublish(List<BubblingObject> bubblingObjects)
        {
            foreach (var bubblingObject in bubblingObjects)
            {
                //todo optimization ho messo la ricezione per la ottimizzatione decommenta  // OffRampStream.SendMessage(bubblingObject);
                if(bubblingObject.LocalEvent)
                    MessageIngestor.IngestMessagge(bubblingObject);
                else
                {
                    // Send message to message provider 
                    OffRampStream.SendMessage(bubblingObject);
                }
            }
        }
    }
}