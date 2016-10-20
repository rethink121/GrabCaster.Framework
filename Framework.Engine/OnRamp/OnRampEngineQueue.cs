// --------------------------------------------------------------------------------------------------
// <copyright file = "OnRampEngine.cs" company="GrabCaster Ltd">
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

using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Messaging;

namespace GrabCaster.Framework.Engine.OnRamp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Log;

    /// <summary>
    /// The on ramp engine.
    /// </summary>
    public sealed class OnRampEngineQueue : LockSlimQueueEngine<BubblingObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnRampEngineQueue"/> class.
        /// </summary>
        /// <param name="capLimit">
        /// The cap limit.
        /// </param>
        /// <param name="timeLimit">
        /// The time limit.
        /// </param>
        public OnRampEngineQueue(int capLimit, int timeLimit)
        {
            this.CapLimit = capLimit;
            this.TimeLimit = timeLimit;
            this.InitTimer();
        }
    }

    /// <summary>
    /// The on ramp engine receiving.
    /// </summary>
    public class OnRampEngineReceiving
    {
        /// <summary>
        /// The parameters ret.
        /// </summary>
        private static readonly object[] ParametersRet = { null };

        /// <summary>
        /// Create the internal queue
        /// </summary>
        private readonly OnRampEngineQueue _onRampEngineQueue;

        private static IOnRampStream OnRampStream;
        /// <summary>
        /// Delegate used to fire the event to enqueue the message.
        /// </summary>
        private SetEventOnRampMessageReceived receiveMessageOnRampDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnRampEngineReceiving"/> class.
        /// </summary>
        public OnRampEngineReceiving()
        {
            this._onRampEngineQueue = new OnRampEngineQueue(
                ConfigurationBag.Configuration.ThrottlingOnRampIncomingRateNumber, 
                ConfigurationBag.Configuration.ThrottlingOnRampIncomingRateSeconds);
            this._onRampEngineQueue.OnPublish += OnRampEngineQueueOnPublish;
        }

        /// <summary>
        /// Initialize the onramp engine.
        /// </summary>
        /// <param name="onRampPatternComponent">
        /// The off ramp pattern component.
        /// </param>
        public void Init(string onRampPatternComponent)
        {
            if (ConfigurationBag.Configuration.RunLocalOnly)
            {

                LogEngine.WriteLog(ConfigurationBag.EngineName,
                                    $"OnRamp provider not started, this GrabCaster point is configured for local execution only.",
                                    Constant.LogLevelError,
                                    Constant.TaskCategoriesError,
                                    null,
                                    Constant.LogLevelWarning);
                return;
            }
            // Delegate event for ingestor where ReceiveMessageOnRamp is the event
            this.receiveMessageOnRampDelegate = this.ReceiveMessageOnRamp;

            LogEngine.WriteLog(
                ConfigurationBag.EngineName, 
                "Start On Ramp engine.", 
                Constant.LogLevelError, 
                Constant.TaskCategoriesError, 
                null, 
                Constant.LogLevelInformation);

            // Inizialize the MSPC

            // Load event up stream external component
            var eventsUpStreamComponent = Path.Combine(
                ConfigurationBag.Configuration.DirectoryOperativeRootExeName, 
                ConfigurationBag.Configuration.EventsStreamComponent);

            // Create the reflection method cached 
            var assembly = Assembly.LoadFrom(eventsUpStreamComponent);

            // Main class loggingCreateOnRamptream
            var assemblyClass = (from t in assembly.GetTypes()
                                 let attributes = t.GetCustomAttributes(typeof(EventsOnRampContract), true)
                                 where t.IsClass && attributes != null && attributes.Length > 0
                                 select t).First();


            OnRampStream = Activator.CreateInstance(assemblyClass) as IOnRampStream;
            OnRampStream.Run(this.receiveMessageOnRampDelegate);

        }

        /// <summary>
        /// Send the message to the engine message.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        private static void OnRampEngineQueueOnPublish(List<BubblingObject> objects)
        {
            foreach (var message in objects)
            {
                // Sent message to the MSPC
                MessageIngestor.IngestMessagge(message);
            }
        }

        /// <summary>
        /// Event fired by the On Ramp engine.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void ReceiveMessageOnRamp(BubblingObject message)
        {
            this._onRampEngineQueue.Enqueue(message);
        }
    }
}