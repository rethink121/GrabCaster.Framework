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

namespace GrabCaster.Framework.Library.Azure
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;



    /// <summary>
    ///     Main Downstream events receiving
    ///     It execute the main DownStream Instance
    /// </summary>
    public class EventsDownStream
    {
        private MessageIngestor.SetEventActionEventEmbedded SetEventActionEventEmbedded;

        public void Run(MessageIngestor.SetEventActionEventEmbedded setEventActionEventEmbedded)
        {
            try
            {
                SetEventActionEventEmbedded = setEventActionEventEmbedded;

                //Load message ingestor
                MessageIngestor.Init(SetEventActionEventEmbedded);
                // Assign the delegate 
           
                // Load vars
                var eventHubConnectionString = ConfigurationLibrary.AzureNameSpaceConnectionString();
                var eventHubName = ConfigurationLibrary.GroupEventHubsName();

                LogEngine.TraceInformation($"Start GrabCaster DownStream - Point Id {ConfigurationLibrary.PointId()} - Point name {ConfigurationLibrary.PointName()} - Channel Id {ConfigurationLibrary.ChannelId()} - Channel name {ConfigurationLibrary.ChannelName()} ");

                var builder = new ServiceBusConnectionStringBuilder(eventHubConnectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                //If not exit it create one, drop brachets because Azure rules
                var eventHubConsumerGroup = string.Concat(ConfigurationLibrary.EngineName(), "_", ConfigurationLibrary.ChannelId().Replace("{", "").Replace("}", "").Replace("-", ""));

                var nsManager = NamespaceManager.CreateFromConnectionString(builder.ToString());

                LogEngine.TraceInformation($"Start DirectRegisterEventReceiving. - Initializing Group Name {eventHubConsumerGroup}");

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

                LogEngine.TraceInformation("After DirectRegisterEventReceiving Downstream running.");
            }
            catch (Exception ex)
            {
                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Hint: Check if the firewall outbound port 5671 is opened. - Error {ex.Message}");
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
                EventHubReceiver receiver = null;
                receiver = group.CreateReceiver(partitionId, DateTime.UtcNow);
                LogEngine.TraceInformation($"Direct Receiver created. Partition {partitionId}");
                while (true)
                {
                    var message = receiver?.Receive();
                    if (message != null)
                    {
                        BubblingObject bubblingObject = BubblingObject.DeserializeMessage(message.GetBytes());
                        MessageIngestor.IngestMessagge(bubblingObject);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Error {ex.Message}");
            }
        }
    }
}