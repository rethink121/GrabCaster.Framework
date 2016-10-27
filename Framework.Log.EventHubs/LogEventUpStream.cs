// LogEventUpStream.cs
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
namespace GrabCaster.Framework.Log.EventHubs
{
    using System;
    using System.Diagnostics;
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Log;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    using Newtonsoft.Json;

    /// <summary>
    ///     Send messages to EH
    /// </summary>
    internal static class LogEventUpStream
    {
        //EH variable

        private static string azureNameSpaceConnectionString = "";

        private static string eventHubName = "";

        private static EventHubClient eventHubClient;

        public static bool CreateEventUpStream()
        {
            try
            {

                Debug.WriteLine("-------------- Engine LogEventUpStream --------------");
                Debug.WriteLine("LogEventUpStream - Get Configuration settings.");
                //Event Hub Configuration
                azureNameSpaceConnectionString = ConfigurationBag.Configuration.AzureNameSpaceConnectionString;
                eventHubName = ConfigurationBag.Configuration.LoggingComponentStorage;
                Debug.WriteLine($"LogEventUpStream - azureNameSpaceConnectionString={azureNameSpaceConnectionString}");
                Debug.WriteLine($"LogEventUpStream - eventHubName={eventHubName}");

                // TODO ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Https;

                var builder = new ServiceBusConnectionStringBuilder(azureNameSpaceConnectionString)
                {
                    TransportType =
                                          TransportType
                                          .Amqp
                };

                Debug.WriteLine("LogEventUpStream - Create the eventHubClient.");

                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LogEventUpStream - error->{0}", ex.Message);
                EventLog.WriteEntry("Framework.Log.EventHubs", ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Send a EventMessage message
        /// </summary>
        public static bool SendMessage(LogMessage logMessage)
        {
            try
            {
                if (ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    EventLog.WriteEntry("Framework.Log.EventHubs", "The remote logging storage provider is not available, this GrabCaster point is configured for local only execution.", EventLogEntryType.Error);
                    return true;
                }

                Debug.WriteLine("LogEventUpStream - serialize log message.");
                //Create EH data message
                var jsonSerialized = JsonConvert.SerializeObject(logMessage);
                var serializedMessage = EncodingDecoding.EncodingString2Bytes(jsonSerialized);

                var data = new EventData(serializedMessage);
                Debug.WriteLine("LogEventUpStream - send log message.");

                //Send the metric to Event Hub
                eventHubClient.Send(data);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LogEventUpStream - error->{0}", ex.Message);

                EventLog.WriteEntry("Framework.Log.EventHubs", ex.Message, EventLogEntryType.Error);
                return false;
            }
        }
    }
}