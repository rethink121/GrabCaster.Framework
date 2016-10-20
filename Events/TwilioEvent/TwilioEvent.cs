// --------------------------------------------------------------------------------------------------
// <copyright file = "TwilioEvent.cs" company="GrabCaster Ltd">
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

namespace GrabCaster.Framework.TwilioEvent
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Twilio;

    /// <summary>
    /// The twilio event.
    /// </summary>
    [EventContract("{A5765B22-4003-4463-AB93-EEB5C0C477FE}", "Twilio Event", "Twilio send text message", true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class TwilioEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the account sid.
        /// </summary>
        [EventPropertyContract("AccountSid", "AccountSid")]
        public string AccountSid { get; set; }

        /// <summary>
        /// Gets or sets the auth token.
        /// </summary>
        [EventPropertyContract("AuthToken", "AuthToken")]
        public string AuthToken { get; set; }

        /// <summary>
        /// Gets or sets the from.
        /// </summary>
        [EventPropertyContract("From", "From")]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the to.
        /// </summary>
        [EventPropertyContract("To", "To")]
        public string To { get; set; }

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
        /// <exception cref="Exception">
        /// </exception>
        [EventActionContract("{5ABB263A-8B69-49F7-BC9E-802A0A81AA0B}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                var content = EncodingDecoding.EncodingBytes2String(this.DataContext);
                var twilio = new TwilioRestClient(this.AccountSid, this.AuthToken);
                var text = content.Replace("\"", string.Empty).Replace("\\", string.Empty);
                twilio.SendMessage(this.From, this.To, text);
                return null;
                // SetEventActionEvent(this, context);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}