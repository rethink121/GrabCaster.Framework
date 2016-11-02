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
using System.Runtime.Serialization;

namespace GrabCaster.Framework.BizTalk.Adapter
{
	internal class GrabCasterExceptions : ApplicationException
	{
		public static string UnhandledTransmit_Error = "The GrabCaster Adapter encounted an error transmitting a batch of messages.";

        public GrabCasterExceptions () { }

		public GrabCasterExceptions (string msg) : base(msg) { }

		public GrabCasterExceptions (Exception inner) : base(String.Empty, inner) { }

		public GrabCasterExceptions (string msg, Exception e) : base(msg, e) { }

		protected GrabCasterExceptions (SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}

