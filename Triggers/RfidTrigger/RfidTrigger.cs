// --------------------------------------------------------------------------------------------------
// <copyright file = "RfidTrigger.cs" company="GrabCaster Ltd">
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

using GrabCaster.Framework.Base;

namespace GrabCaster.Framework.RfidTrigger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    using Newtonsoft.Json;

    using Phidgets;
    using Phidgets.Events;

    /// <summary>
    /// The RIFD trigger.
    /// </summary>
    [TriggerContract("{782B745E-1F6F-440A-A209-E250A1EA5013}", "RFID Trigger", "Read Rfid Tag from a Pidget device",
        false, true, false)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class RfidTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the event message.
        /// </summary>
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
        [TriggerActionContract("{76438098-8811-4F14-825A-F0B8AB932465}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                this.Context = context;
                this.ActionTrigger = actionTrigger;

                var rfid = new RFID(); // Declare an RFID object

                // initialize our Phidgets RFID reader and hook the event handlers
                rfid.Attach += RfidAttach;
                rfid.Detach += RfidDetach;
                rfid.Error += this.RfidError;

                rfid.Tag += this.RfidTag;
                rfid.TagLost += RfidTagLost;
                rfid.open();

                // Wait for a Phidget RFID to be attached before doing anything with 
                // the object
                rfid.waitForAttachment();

                // turn on the antenna and the led to show everything is working
                rfid.Antenna = true;
                rfid.LED = true;
                Thread.Sleep(Timeout.Infinite);
                return null;
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }

        /// <summary>
        /// Attach event handler. display the serial number of the attached RFID phidget
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void RfidAttach(object sender, AttachEventArgs e)
        {
            // NOP
        }

        /// <summary>
        /// Detach event handler. display the serial number of the detached RFID phidget
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void RfidDetach(object sender, DetachEventArgs e)
        {
            // NOP
        }

        /// <summary>
        /// Print the tag code for the tag that was just lost
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void RfidTagLost(object sender, TagEventArgs e)
        {
            // NOP
        }

        /// <summary>
        /// Error event handler. display the error description string
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RfidError(object sender, ErrorEventArgs e)
        {
            this.DataContext = EncodingDecoding.EncodingString2Bytes(e.Description);
            this.ActionTrigger(this, this.Context);
        }

        /// <summary>
        /// Print the tag code of the scanned tag
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RfidTag(object sender, TagEventArgs e)
        {
            var rfidTag = new RfidTag { TagId = e.Tag, BankId = "4433EB52-240A-44CC-8A3B-B6673E1E0B31" };

            var tagString = JsonConvert.SerializeObject(rfidTag);

            this.DataContext = EncodingDecoding.EncodingString2Bytes(tagString);
            this.ActionTrigger(this, this.Context);
        }
    }

    /// <summary>
    /// The rfid tag.
    /// </summary>
    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class RfidTag
    {
        /// <summary>
        /// Gets or sets the tag id.
        /// </summary>
        [DataMember]
        public string TagId { get; set; }

        /// <summary>
        /// Gets or sets the bank id.
        /// </summary>
        [DataMember]
        public string BankId { get; set; }
    }
}