// --------------------------------------------------------------------------------------------------
// <copyright file = "Globals.cs" company="GrabCaster Ltd">
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

namespace GrabCaster.Framework.Contracts.Globals
{
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Contracts.Triggers;



    /// <summary>
    /// Action that will be executed by the event in SyncAsync scenarios
    /// </summary>
    /// <param name="dataContext"></param>
    public delegate void SyncAsyncEventAction(byte[] dataContext);

    /// <summary>
    /// Global Action Trigger Delegate used by triggers.
    /// </summary>
    /// <param name="_this">
    /// The _this.
    /// </param>
    /// <param name="context">
    /// The context.
    /// </param>
    public delegate void ActionTrigger(ITriggerType _this, ActionContext context);

    /// <summary>
    /// Global Action Event Delegate used by events.
    /// </summary>
    /// <param name="_this">
    /// The _this.
    /// </param>
    /// <param name="context">
    /// The context.
    /// </param>
    public delegate void ActionEvent(IEventType _this, ActionContext context);

    /// <summary>
    /// Global On Ramp Delegate used by on ramp receiver.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public delegate void SetEventOnRampMessageReceived(BubblingObject message);

    /// <summary>
    /// Global Off Ramp Delegate used by on ramp sender.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public delegate void SetEventOnRampMessageSent(object message);


    /// <summary>
    /// Context exchanged between triggers and events
    /// </summary>
    public class ActionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContext"/> class.
        /// </summary>
        /// <param name="bubblingObjectBag">
        /// The bubbling configuration.
        /// </param>
        public ActionContext(BubblingObject bubblingObjectBag)
        {
            this.BubblingObjectBag = bubblingObjectBag;
        }

        /// <summary>
        /// Bag used to transport the triggers and events
        /// </summary>
        public BubblingObject BubblingObjectBag { get; set; }

        /// <summary>
        /// MessageId sent across the points
        /// </summary>
        public string MessageId { get; set; }

    }
}