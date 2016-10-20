// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsDownStream.cs" company="GrabCaster Ltd">
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

namespace GrabCaster.Framework.Dcp.Azure
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Log;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    ///     Main Downstream events receiving
    ///     It execute the main DownStream Instance
    /// </summary>
    [EventsOnRampContract("{B8ECF14B-2A9E-41C9-9E85-D8EA2D5C4E22}", "EventsDownStream", "Event Hubs EventsDownStream")]
    public class OnRampStream : IOnRampStream
    {
        private static SetEventOnRampMessageReceived SetEventOnRampMessageReceived { get; set; }


        public void Run(SetEventOnRampMessageReceived setEventOnRampMessageReceived)
        {
            try
            {

                // Assign the delegate 
                SetEventOnRampMessageReceived = setEventOnRampMessageReceived;
                // Load vars
                var eventHubConnectionString = ConfigurationBag.Configuration.AzureNameSpaceConnectionString;
                var eventHubName = ConfigurationBag.Configuration.GroupEventHubsName;

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Event Hubs transfort Type: {ConfigurationBag.Configuration.ServiceBusConnectivityMode}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    null,
                    Constant.LogLevelInformation);

                var builder = new ServiceBusConnectionStringBuilder(eventHubConnectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                //If not exit it create one, drop brachets because Azure rules
                var eventHubConsumerGroup =
                    string.Concat(ConfigurationBag.EngineName, "_", ConfigurationBag.Configuration.ChannelId)
                        .Replace("{", "")
                        .Replace("}", "")
                        .Replace("-", "");
                var nsManager = NamespaceManager.CreateFromConnectionString(builder.ToString());
                Debug.WriteLine(
                    $"Initializing Group Name {eventHubConsumerGroup}",
                    ConsoleColor.White);

                Debug.WriteLine("Start DirectRegisterEventReceiving.", ConsoleColor.White);

                // Create Event Hubs
                var eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
                // Create consumer
                nsManager.CreateConsumerGroupIfNotExists(eventHubName, eventHubConsumerGroup);

                var namespaceManager = NamespaceManager.CreateFromConnectionString(builder.ToString());
                var ehDescription = namespaceManager.GetEventHub(eventHubName);
                // Use the default consumer group

                foreach (var partitionId in ehDescription.PartitionIds)
                {
                    var myNewThread =
                        new Thread(() => ReceiveDirectFromPartition(eventHubClient, partitionId, eventHubConsumerGroup));
                    myNewThread.Start();
                }

                Debug.WriteLine(
                    "After DirectRegisterEventReceiving Downstream running.",
                    ConsoleColor.White);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name} - Hint: Check if the firewall outbound port 5671 is opened.",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    Constant.LogLevelError);
            }
        }

        private static void ReceiveDirectFromPartition(
            EventHubClient eventHubClient,
            string partitionId,
            string consumerGroup)
        {
            try
            {
                var group = eventHubClient.GetConsumerGroup(consumerGroup);

                var eventHubsStartingDateTimeReceiving =
                    DateTime.Parse(
                        ConfigurationBag.Configuration.EventHubsStartingDateTimeReceiving == "0"
                            // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                            ? DateTime.UtcNow.ToString()
                            : ConfigurationBag.Configuration.EventHubsStartingDateTimeReceiving);
                var eventHubsEpoch = ConfigurationBag.Configuration.EventHubsEpoch;

                EventHubReceiver receiver = null;

                switch (ConfigurationBag.Configuration.EventHubsCheckPointPattern)
                {
                    case EventHubsCheckPointPattern.CheckPoint:
                        //Receiving from the last valid receiving point
                        receiver = group.CreateReceiver(partitionId, DateTime.UtcNow);
                        break;
                    case EventHubsCheckPointPattern.Dt:

                        receiver = group.CreateReceiver(partitionId, eventHubsStartingDateTimeReceiving);
                        break;
                    case EventHubsCheckPointPattern.Dtepoch:
                        receiver = group.CreateReceiver(partitionId, eventHubsStartingDateTimeReceiving, eventHubsEpoch);
                        break;
                    case EventHubsCheckPointPattern.Dtutcnow:
                        receiver = group.CreateReceiver(partitionId, DateTime.UtcNow);
                        break;
                    case EventHubsCheckPointPattern.Dtnow:
                        receiver = group.CreateReceiver(partitionId, DateTime.Now);
                        break;
                    case EventHubsCheckPointPattern.Dtutcnowepoch:
                        receiver = group.CreateReceiver(partitionId, DateTime.UtcNow, eventHubsEpoch);
                        break;
                    case EventHubsCheckPointPattern.Dtnowepoch:
                        receiver = group.CreateReceiver(partitionId, DateTime.Now, eventHubsEpoch);
                        break;
                }
                Debug.WriteLine(
                    $"Direct Receiver created. Partition {partitionId}",
                    ConsoleColor.Yellow);
                while (true)
                {
                    var message = receiver?.Receive();
                    if (message != null)
                    {
                        BubblingObject bubblingObject = BubblingObject.DeserializeMessage(message.GetBytes());
                        SetEventOnRampMessageReceived(bubblingObject);
                    }
                }
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
            }
        }
    }
}