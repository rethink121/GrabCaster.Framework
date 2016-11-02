// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
using System;
using System.Xml;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter.Common
{
    public abstract class EndpointParameters
    {
        public abstract string SessionKey { get; }
        public string OutboundLocation { get { return this.outboundLocation; } }
        public EndpointParameters (string outboundLocation)
        {
            this.outboundLocation = outboundLocation;
        }
        protected string outboundLocation;
    }

    internal class DefaultEndpointParameters : EndpointParameters
    {
        public override string SessionKey 
        {
            //  the SessionKey is the outboundLocation in the default case
            get { return this.outboundLocation; }
        }
        public DefaultEndpointParameters (string outboundLocation) : base(outboundLocation)
        {
        }
    }

    public abstract class AsyncTransmitterEndpoint : System.IDisposable
    {
        public AsyncTransmitterEndpoint(AsyncTransmitter transmitter) { }

        public virtual bool ReuseEndpoint { get { return true; } }
        public abstract void Open (EndpointParameters endpointParameters, IPropertyBag handlerPropertyBag, string propertyNamespace);
        public abstract IBaseMessage ProcessMessage (IBaseMessage message);
        public virtual void Dispose () { }
    }
}
