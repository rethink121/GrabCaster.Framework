// AzureQueueEvent.cs
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

using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

#endregion

namespace GrabCaster.Framework.AzureQueueEvent
{
    /// <summary>
    ///     The azure queue event.
    /// </summary>
    [EventContract("{628CB14D-7F85-4D99-8EC6-489EBA25C38A}", "AzureQueueEvent", "Send message to Azure Queue", true)]
    public class AzureQueueEvent : IEventType
    {
        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        [EventPropertyContract("ConnectionString", "Azure ConnectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the queue path.
        /// </summary>
        [EventPropertyContract("QueuePath", "QueuePath")]
        public string QueuePath { get; set; }

        /// <summary>
        ///     Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        ///     Gets or sets the set event action event.
        /// </summary>
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        ///     Gets or sets the data context.
        /// </summary>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        ///     The execute.
        /// </summary>
        /// <param name="actionEvent">
        ///     The set event action event.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        [EventActionContract("{287F60BE-B257-4EE8-B3C3-328D0AFCD692}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                Context = context;
                ActionEvent = actionEvent;

                var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);

                if (!namespaceManager.QueueExists(QueuePath))
                {
                    namespaceManager.CreateQueue(QueuePath);
                }

                var client = QueueClient.CreateFromConnectionString(ConnectionString, QueuePath);
                client.Send(new BrokeredMessage(DataContext));
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
}