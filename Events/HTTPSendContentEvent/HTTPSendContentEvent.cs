// HTTPSendContentEvent.cs
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

using System.IO;
using System.Net;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;

#endregion

namespace HTTPSendContentEvent.Event
{
    /// <summary>
    ///     The no operation event.
    /// </summary>
    [EventContract("{8c87cf14-7a9c-4a62-91b5-d47cd57695d8}", "HTTPSendContentEvent",
         "HTTPSendContentEvent Event component", true)]
    public class HTTPSendContentEvent : IEventType
    {
        [EventPropertyContract("url", "url property")]
        public string url { get; set; }

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
        [EventPropertyContract("DataContext", "Main data context")]
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
        [EventActionContract("{83029d5b-dd61-4184-a884-f3b937ce2da1}", "Main action",
             "Main action executed by the event")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            // declare httpwebrequet wrt url defined above
            HttpWebRequest webrequest = (HttpWebRequest) WebRequest.Create(url);
            // set method as post
            webrequest.Method = "POST";
            // set content type
            webrequest.ContentType = "application/soap+xml";
            // set content length
            webrequest.ContentLength = DataContext.Length;
            // get stream data out of webrequest object
            Stream newStream = webrequest.GetRequestStream();
            newStream.Write(DataContext, 0, DataContext.Length);
            newStream.Close();
            // declare & read response from service
            HttpWebResponse webresponse = (HttpWebResponse) webrequest.GetResponse();


            actionEvent(this, context);
            return null;
        }
    }
}