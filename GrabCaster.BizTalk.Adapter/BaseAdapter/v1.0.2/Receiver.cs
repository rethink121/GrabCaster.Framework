// Receiver.cs
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
using System;
using System.Collections.Generic;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter.Common
{
    /// <summary>
    /// Abstract base class for receiver adapters. Provides stock implementations of
    /// core interfaces needed to comply with receiver adapter contract.
    /// (1) This class is actually a Singleton. That is there will only ever be one
    /// instance of it created however many locations of this type are actually defined.
    /// (2) Individual locations are identified by a URI, the derived class must provide
    /// the Type name and this class acts as a Factory using the .NET Activator
    /// (3) It is legal to have messages from different locations submitted in a single
    /// batch and this may be an important optimization. And this is fundamentally why
    /// the Receiver is a singleton.
    /// </summary>
    public abstract class Receiver :
        Adapter,
        IBTTransportConfig,
        IDisposable
    {
        //  core member data
        private Dictionary<string, ReceiverEndpoint> endpoints = new Dictionary<string, ReceiverEndpoint>(StringComparer.OrdinalIgnoreCase);
        private Type endpointType;

        private ControlledTermination control;

        protected Receiver (
            string name,
            string version,
            string description,
            string transportType,
            Guid clsid,
            string propertyNamespace,
            Type endpointType)
        : base(
            name,
            version,
            description,
            transportType,
            clsid,
            propertyNamespace)
        {
			this.endpointType = endpointType;
            this.control = new ControlledTermination();
		}

        public void Dispose()
        {
            this.control.Dispose();
        }

        //  IBTTransportConfig
        public void AddReceiveEndpoint (string Url, IPropertyBag pConfig, IPropertyBag pBizTalkConfig)
        {
            if (!this.Initialized)
                throw new NotInitialized();

            if (this.endpoints.ContainsKey(Url))
                throw new EndpointExists(Url);

            ReceiverEndpoint endpoint = (ReceiverEndpoint)Activator.CreateInstance(this.endpointType);

            if (null == endpoint)
                throw new CreateEndpointFailed(this.endpointType.FullName, Url);

            endpoint.Open(Url, pConfig, pBizTalkConfig, this.HandlerPropertyBag, this.TransportProxy, this.TransportType, this.PropertyNamespace, this.control);

            this.endpoints[Url] = endpoint;
        }
        public void UpdateEndpointConfig (string Url, IPropertyBag pConfig, IPropertyBag pBizTalkConfig)
        {
            if (!this.Initialized)
                throw new NotInitialized();

            ReceiverEndpoint endpoint = (ReceiverEndpoint)this.endpoints[Url];

            if (null == endpoint)
                throw new EndpointNotExists(Url);

            //  delegate the update call to the endpoint instance itself
            endpoint.Update(pConfig, pBizTalkConfig, this.HandlerPropertyBag);
		}
        public void RemoveReceiveEndpoint (string Url)
        {
			if (!this.Initialized)
				throw new NotInitialized();

			ReceiverEndpoint endpoint = (ReceiverEndpoint)this.endpoints[Url];

			if (null == endpoint)
				return;

			this.endpoints.Remove(Url);
			endpoint.Dispose();
		}

        public ReceiverEndpoint GetEndpoint(string url)
        {
            ReceiverEndpoint endpoint;
            this.endpoints.TryGetValue(url, out endpoint);

            return endpoint;
        }

        //  IBTransportControl
        public override void Initialize (IBTTransportProxy transportProxy)
        {
			base.Initialize(transportProxy);
		}

        public override void Terminate ()
        {
            try
            {
                base.Terminate();
                foreach (ReceiverEndpoint endpoint in this.endpoints.Values)
                {
                    endpoint.Dispose();
                }
                this.endpoints.Clear();
                this.endpoints = null;

                //  Block until we are done...
                this.control.Terminate();
            }
            finally
            {
                this.Dispose();
            }
		}
    }
}
