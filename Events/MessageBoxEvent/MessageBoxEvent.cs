// --------------------------------------------------------------------------------------------------
// <copyright file = "MessageBoxEvent.cs" company="GrabCaster Ltd">
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

namespace GrabCaster.Framework.MessageBoxEvent
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Windows.Forms;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The message box event.
    /// </summary>
    [EventContract("{DC78440A-E82B-4A5C-B4C5-C78CC40472BD}", "MessageBox Event", "Show a messagebox", true)]
    public class MessageBoxEvent : IEventType
    {
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
        [EventActionContract("{1B28FEDB-3940-463D-BA80-223B4C40FE31}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                Debug.WriteLine("In MessageBoxEvent Event.");
                var message = EncodingDecoding.EncodingBytes2String(this.DataContext);
                MessageBox.Show(message);
                actionEvent(this, context);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MessageBoxEvent error > " + ex.Message);
                actionEvent(this, null);
                return null;
            }
        }
    }
}