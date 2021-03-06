﻿// PageOneMessageEvent.cs
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

using System;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;

#endregion

namespace HM.OMS.PageOneMessageEvent
{
    /// <summary>
    ///     The file event.
    /// </summary>
    [EventContract("{BC76EEB8-369E-4B0B-BC52-4DFBD4FA33B1}", "PageOneMessageEvent",
         "PageOneMessageEvent is the OMS service to send mails", true)]
    public class PageOneMessageEvent : IEventType
    {
        /// <summary>
        ///     [message] to send an email - [auth] to check the authentication
        /// </summary>
        [EventPropertyContract("input", "[message] to send an email - [auth] to check the authentication")]
        public string input { get; set; }

        /// <summary>
        ///     [message] to send an email - [auth] to check the authentication
        /// </summary>
        [EventPropertyContract("From", "The mail from")]
        public string From { get; set; }

        /// <summary>
        ///     [message] to send an email - [auth] to check the authentication
        /// </summary>
        [EventPropertyContract("To", "The mail to")]
        public string To { get; set; }

        /// <summary>
        ///     [message] to send an email - [auth] to check the authentication
        /// </summary>
        [EventPropertyContract("Message", "Body message")]
        public string Message { get; set; }

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
        [EventActionContract("{E4FD267F-92B2-45F3-B198-0F1581E2EBBA}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                DataContext = EncodingDecoding.EncodingString2Bytes(To);
                //Console.WriteLine("In event before handle");
                //Console.WriteLine($"EVENT {context.BubblingConfiguration.MessageId} - {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}");

                actionEvent(this, context);

                return null;
            }
            catch (Exception ex)
            {
                DataContext = EncodingDecoding.EncodingString2Bytes(ex.Message);
                ;
                actionEvent(this, context);
                return null;
            }
        }
    }
}