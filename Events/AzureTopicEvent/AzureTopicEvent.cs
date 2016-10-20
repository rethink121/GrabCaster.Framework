// --------------------------------------------------------------------------------------------------
// <copyright file = "AzureTopicEvent.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.AzureTopicEvent
{
    using System;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The azure topic event.
    /// </summary>
    [EventContract("{B2311010-B505-4F9F-A927-2035A7640BCB}", "AzureTopicEvent", "Send message to Azure Topic", true)]
    public class AzureTopicEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [EventPropertyContract("ConnectionString", "Azure ConnectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the topic path.
        /// </summary>
        [EventPropertyContract("TopicPath", "TopicPath")]
        public string TopicPath { get; set; }

        /// <summary>
        /// Gets or sets the message context properties.
        /// </summary>
        [EventPropertyContract("MessageContextProperties", "MessageContextProperties")]
        public string MessageContextProperties { get; set; }

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
        [EventActionContract("{D33251EF-7638-4C34-AD6B-B5CBE32F7056}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                this.Context = context;
                this.ActionEvent = actionEvent;

                var namespaceManager = NamespaceManager.CreateFromConnectionString(this.ConnectionString);

                if (!namespaceManager.TopicExists(this.TopicPath))
                {
                    namespaceManager.CreateTopic(this.TopicPath);
                }

                var client = TopicClient.CreateFromConnectionString(this.ConnectionString, this.TopicPath);
                var brokeredMessage = new BrokeredMessage(this.DataContext);

                var value = this.MessageContextProperties.Split('|');
                brokeredMessage.Properties[value[0]] = value[1];
                client.Send(brokeredMessage);
                actionEvent(this, context);
                return null;
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }
    }
}