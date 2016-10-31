// OnRampstream.cs
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
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Messaging;
using GrabCaster.Framework.Log;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

#endregion

namespace GrabCaster.Framework.Dcp.Azure
{
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
                Debug.WriteLine($"Initializing Group Name {eventHubConsumerGroup}");

                Debug.WriteLine("Start DirectRegisterEventReceiving.");

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
                    "After DirectRegisterEventReceiving Downstream running.");
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