// --------------------------------------------------------------------------------------------------
// <copyright file = "EventHubEvent.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.EventHubEvent
{
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The event hub event.
    /// </summary>
    [EventContract("{F249290E-0231-44A9-A348-1CC7FCC33C7F}", "Event Hub Event", "Send a message to Azure Event Hub.", true)]
    public class EventHubEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the event hub name.
        /// </summary>
        [EventPropertyContract("EventHubName", "EventHubName")]
        public string EventHubName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [EventPropertyContract("ConnectionString", "Event Hub connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action event.
        /// </summary>
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="actionEvent">
        /// The set event action event.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [EventActionContract("{FA452E1A-95E9-4076-A1EE-1B41E9561824}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                if (!InternalEventUpStream.InstanceLoaded)
                {
                    InternalEventUpStream.CreateEventUpStream(this.ConnectionString, this.EventHubName);
                    InternalEventUpStream.InstanceLoaded = true;
                }

                InternalEventUpStream.SendMessage(this.DataContext);
                actionEvent(this, context);
                return null;
            }
            catch
            {
                // ignored
                return null;
            }
        }
    }

    /// <summary>
    /// The internal event up stream.
    /// </summary>
    internal static class InternalEventUpStream
    {
        /// <summary>
        /// The builder.
        /// </summary>
        private static ServiceBusConnectionStringBuilder builder;

        /// <summary>
        /// The event hub client.
        /// </summary>
        private static EventHubClient eventHubClient;

        /// <summary>
        /// The instance loaded.
        /// </summary>
        public static bool InstanceLoaded { get; set; }

        /// <summary>
        /// The create event up stream.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="eventHubName">
        /// The event hub name.
        /// </param>
        public static void CreateEventUpStream(string connectionString, string eventHubName)
        {
            try
            {
                builder = new ServiceBusConnectionStringBuilder(connectionString) { TransportType = TransportType.Amqp };
                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void SendMessage(byte[] message)
        {
            try
            {
                var data = new EventData(message);
                eventHubClient.SendAsync(data);
            }
            catch
            {
                // ignored
            }
        }
    }
}