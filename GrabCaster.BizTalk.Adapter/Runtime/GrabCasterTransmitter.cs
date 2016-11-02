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
using Microsoft.BizTalk.Component.Interop;
using GrabCaster.Framework.BizTalk.Adapter.Common;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter
{
	/// <summary>
	/// This is a singleton class for GrabCaster send adapter. All the messages, going to various
	/// send ports of this adapter type, will go through this class.
	/// </summary>
	public class GrabCasterTransmitter : AsyncTransmitter
	{
		internal static string GRABCASTER_NAMESPACE = "http://schemas.microsoft.com/BizTalk/2003/SDK_Samples/Messaging/Transports/grabcaster-properties";

		public GrabCasterTransmitter() : base(
			"GrabCaster Transmit Adapter",
			"1.0",
			"Send messages form BizTalk to GrabCaster points",
            "GrabCaster-Messaging",
			new Guid("024DB758-AAF9-415e-A121-4AC245DD49EC"),
            GRABCASTER_NAMESPACE,
			typeof(GrabCasterTransmitterEndpoint),
            GrabCasterTransmitProperties.BatchSize)
        {
		}
	
		protected override void HandlerPropertyBagLoaded ()
		{
			IPropertyBag config = this.HandlerPropertyBag;
			if (null != config)
			{
				XmlDocument handlerConfigDom = ConfigProperties.IfExistsExtractConfigDom(config);
				if (null != handlerConfigDom)
				{
					GrabCasterTransmitProperties.ReadTransmitHandlerConfiguration(handlerConfigDom);
				}
			}
		}
	}
}
