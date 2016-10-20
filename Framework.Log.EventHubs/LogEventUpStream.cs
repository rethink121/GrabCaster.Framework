// --------------------------------------------------------------------------------------------------
// <copyright file = "LogEventUpStream.cs" company="GrabCaster Ltd">
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