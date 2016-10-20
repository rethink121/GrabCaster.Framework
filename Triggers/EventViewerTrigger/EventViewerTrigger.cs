// --------------------------------------------------------------------------------------------------
// <copyright file = "EventViewerTrigger.cs" company="GrabCaster Ltd">
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

using System.Threading;
using GrabCaster.Framework.Base;

namespace GrabCaster.Framework.EventViewerTrigger
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Text;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    using Newtonsoft.Json;

    /// <summary>
    /// The event viewer trigger.
    /// </summary>
    [TriggerContract("{0E8D9421-E749-4B0D-ADCE-03D4A6568998}", "Event Viewer Trigger", "Intercept Event Viewer Message",
        false, true, false)]
    public class EventViewerTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the event log.
        /// </summary>
        [TriggerPropertyContract("EventLog", "Event Source to monitor")]
        public string EventLog { get; set; }

        /// <summary>
        /// Gets or sets the event message.
        /// </summary>
        [TriggerPropertyContract("EventMessage", "Event Message for the Event Viewer")]
        public string EventMessage { get; set; }

        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }

        public string SupportBag { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action trigger.
        /// </summary>
        public ActionTrigger ActionTrigger { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="actionTrigger">
        /// The set event action trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [TriggerActionContract("{25F85716-1154-4473-AFFE-F8F4E8AC17A9}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                this.Context = context;
                this.ActionTrigger = actionTrigger;

                var myNewLog = new EventLog { Log = this.EventLog };

                myNewLog.EntryWritten += this.MyOnEntryWritten;
                myNewLog.EnableRaisingEvents = true;
                return null;
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }

        /// <summary>
        /// The my on entry written.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        public void MyOnEntryWritten(object source, EntryWrittenEventArgs e)
        {
            if (e.Entry.Source != "DEMOEV") return;
            var eventViewerMessage = new EventViewerMessage
                                         {
                                             EntryType = e.Entry.EntryType,
                                             MachineName = e.Entry.MachineName,
                                             Message = e.Entry.Message,
                                             Source = e.Entry.Source,
                                             TimeWritten = e.Entry.TimeWritten
                                         };
            var serializedMessage = JsonConvert.SerializeObject(eventViewerMessage);
            
            this.DataContext = EncodingDecoding.EncodingString2Bytes(serializedMessage);
            this.ActionTrigger(this, this.Context);
        }
    }

    /// <summary>
    /// The event viewer message.
    /// </summary>
    [DataContract]
    internal class EventViewerMessage
    {
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the machine name.
        /// </summary>
        [DataMember]
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the entry type.
        /// </summary>
        [DataMember]
        public EventLogEntryType EntryType { get; set; }

        /// <summary>
        /// Gets or sets the time written.
        /// </summary>
        [DataMember]
        public DateTime TimeWritten { get; set; }
    }
}