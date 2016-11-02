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
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter
{
	class BatchMessage
	{
		private IBaseMessage message;
		private object userData;
		private string correlationToken;
		private BatchOperationType operationType;

		public BatchMessage (IBaseMessage message, object userData, BatchOperationType oppType)
		{
			this.message = message;
			this.userData = userData;
			this.operationType = oppType;
		}

		public BatchMessage (string correlationToken, object userData, BatchOperationType oppType)
		{
			this.correlationToken = correlationToken;
			this.userData = userData;
			this.operationType = oppType;
		}

		public IBaseMessage Message
		{
			get { return this.message; }
		}
		public object UserData
		{
			get { return this.userData; }
		}
		public string CorrelationToken
		{
			get { return this.correlationToken; }
		}
		public BatchOperationType OperationType
		{
			get { return this.operationType; }
		}
	}
}