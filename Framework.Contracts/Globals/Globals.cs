﻿// Globals.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Triggers;

#endregion

namespace GrabCaster.Framework.Contracts.Globals
{
    /// <summary>
    ///     Action that will be executed by the event in SyncAsync scenarios
    /// </summary>
    /// <param name="dataContext"></param>
    public delegate void SyncAsyncEventAction(byte[] dataContext);

    /// <summary>
    ///     Global Action Trigger Delegate used by triggers.
    /// </summary>
    /// <param name="_this">
    ///     The _this.
    /// </param>
    /// <param name="context">
    ///     The context.
    /// </param>
    public delegate void ActionTrigger(ITriggerType _this, ActionContext context);

    /// <summary>
    ///     Global Action Event Delegate used by events.
    /// </summary>
    /// <param name="_this">
    ///     The _this.
    /// </param>
    /// <param name="context">
    ///     The context.
    /// </param>
    public delegate void ActionEvent(IEventType _this, ActionContext context);

    /// <summary>
    ///     Global On Ramp Delegate used by on ramp receiver.
    /// </summary>
    /// <param name="message">
    ///     The message.
    /// </param>
    public delegate void SetEventOnRampMessageReceived(BubblingObject message);

    /// <summary>
    ///     Global Off Ramp Delegate used by on ramp sender.
    /// </summary>
    /// <param name="message">
    ///     The message.
    /// </param>
    public delegate void SetEventOnRampMessageSent(object message);


    /// <summary>
    ///     Context exchanged between triggers and events
    /// </summary>
    public class ActionContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionContext" /> class.
        /// </summary>
        /// <param name="bubblingObjectBag">
        ///     The bubbling configuration.
        /// </param>
        public ActionContext(BubblingObject bubblingObjectBag)
        {
            BubblingObjectBag = bubblingObjectBag;
        }

        /// <summary>
        ///     Bag used to transport the triggers and events
        /// </summary>
        public BubblingObject BubblingObjectBag { get; set; }

        /// <summary>
        ///     MessageId sent across the points
        /// </summary>
        public string MessageId { get; set; }
    }
}