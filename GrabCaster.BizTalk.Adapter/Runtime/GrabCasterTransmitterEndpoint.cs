// GrabCasterTransmitterEndpoint.cs
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

using System.IO;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using GrabCaster.Framework.BizTalk.Adapter.Common;
using GrabCaster.Framework.Contracts.Events;
using System.Diagnostics;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Library;

namespace GrabCaster.Framework.BizTalk.Adapter
{
    /// <summary>
	/// There is one instance of HttpTransmitterEndpoint class for each every static send port.
	/// Messages will be forwarded to this class by AsyncTransmitterBatch
	/// </summary>
	internal class GrabCasterTransmitterEndpoint : AsyncTransmitterEndpoint
	{
		private AsyncTransmitter asyncTransmitter = null;
        private string propertyNamespace;
        private static int IO_BUFFER_SIZE = 4096;
        private const string PROP_REMOTEMESSAGEID = "RemoteMessageId";
        private const string PROP_IDCONFIGURATION = "idConfiguration";
        private const string PROP_IDCOMPONENT = "idComponent";
        private const string PROP_POINTNAME = "pointName";
        private const string PROP_NAMESPACE = "https://GrabCaster.BizTalk.Schemas.GrabCasterProperties";

        public GrabCasterTransmitterEndpoint(AsyncTransmitter asyncTransmitter) : base(asyncTransmitter)
		{
			this.asyncTransmitter = asyncTransmitter;
		}
        /// <summary>
        /// The set event action event embedded.
        /// </summary>
        private static ActionEvent actionEvent;

        public override void Open(EndpointParameters endpointParameters, IPropertyBag handlerPropertyBag, string propertyNamespace)
        {

            actionEvent = EventReceivedFromEmbedded;
            GrabCaster.Framework.Library.Embedded.InitializeOffRampEmbedded(actionEvent);
            this.propertyNamespace = propertyNamespace;
        }

        /// <summary>
        /// The event received from embedded.
        /// </summary>
        /// <param name="eventType">
        /// The event type.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        private void EventReceivedFromEmbedded(IEventType eventType, ActionContext context)
        {
            System.Diagnostics.Debug.WriteLine("Event executed in GrabCaster BizTalk Sender Adpater.");
            Trace.WriteLine("Event executed in GrabCaster BizTalk Sender Adpater.");


        }
        /// <summary>
        /// Implementation for AsyncTransmitterEndpoint::ProcessMessage
        /// Transmit the message and optionally return the response message (for Request-Response support)
        /// </summary>
        public override IBaseMessage ProcessMessage(IBaseMessage message)
		{
           
            Stream source = message.BodyPart.Data;
            byte[] content = null;
            using (var memoryStream = new MemoryStream())
            {
                source.CopyTo(memoryStream);
                content = memoryStream.ToArray();
            }
            // build url
            GrabCasterTransmitProperties props = new GrabCasterTransmitProperties(message, propertyNamespace);

            var idComponent = message.Context.Read(PROP_IDCOMPONENT, PROP_NAMESPACE);
            var idConfiguration = message.Context.Read(PROP_IDCONFIGURATION, PROP_NAMESPACE);
            var pointName = message.Context.Read(PROP_POINTNAME, PROP_NAMESPACE);

            if(idComponent != null && idConfiguration != null)
            {
                GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
                    idConfiguration.ToString(),
                    idComponent.ToString(),
                    content);
            }
            else
            {
                GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
                    props.IdConfiguration,
                    props.IdComponent,
                    content);
            }

            //GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
            //    "{82208FAA-272E-48A7-BB5C-4EACDEA538D2}",
            //    "{306DE168-1CEF-4D29-B280-225B5D0D76FD}",
            //    content);

            return null;
		}

	}
}
