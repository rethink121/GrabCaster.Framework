// OnRampEngineQueue.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
//   - Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//   - Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
//   
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Messaging;

namespace GrabCaster.Framework.Engine.OnRamp
{
    using Base;
    using Contracts.Attributes;
    using Contracts.Globals;
    using Log;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

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
            CapLimit = capLimit;
            TimeLimit = timeLimit;
            InitTimer();
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
        private static readonly object[] ParametersRet = {null};

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
            _onRampEngineQueue = new OnRampEngineQueue(
                ConfigurationBag.Configuration.ThrottlingOnRampIncomingRateNumber,
                ConfigurationBag.Configuration.ThrottlingOnRampIncomingRateSeconds);
            _onRampEngineQueue.OnPublish += OnRampEngineQueueOnPublish;
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
            receiveMessageOnRampDelegate = ReceiveMessageOnRamp;

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
            OnRampStream.Run(receiveMessageOnRampDelegate);
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
            _onRampEngineQueue.Enqueue(message);
        }
    }
}