// EventsDownStream.cs
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
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

#region Usings

using System;
using System.Reflection;
using System.Threading;
using GrabCaster.Framework.Contracts.Bubbling;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

#endregion

namespace GrabCaster.Framework.Library.Azure
{
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

                LogEngine.TraceInformation(
                    $"Start GrabCaster DownStream - Point Id {ConfigurationLibrary.PointId()} - Point name {ConfigurationLibrary.PointName()} - Channel Id {ConfigurationLibrary.ChannelId()} - Channel name {ConfigurationLibrary.ChannelName()} ");

                var builder = new ServiceBusConnectionStringBuilder(eventHubConnectionString)
                {
                    TransportType =
                        TransportType.Amqp
                };

                //If not exit it create one, drop brachets because Azure rules
                var eventHubConsumerGroup = string.Concat(ConfigurationLibrary.EngineName(), "_",
                    ConfigurationLibrary.ChannelId().Replace("{", "").Replace("}", "").Replace("-", ""));

                var nsManager = NamespaceManager.CreateFromConnectionString(builder.ToString());

                LogEngine.TraceInformation(
                    $"Start DirectRegisterEventReceiving. - Initializing Group Name {eventHubConsumerGroup}");

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
                LogEngine.TraceError(
                    $"Error in {MethodBase.GetCurrentMethod().Name} - Hint: Check if the firewall outbound port 5671 is opened. - Error {ex.Message}");
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