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
using System.IO;
using System.Transactions;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter.Common
{
	//  Anything fails in this batch we abort the lot!
	public sealed class AbortOnFailureReceiveTxnBatch : TxnBatch
	{
		public delegate void TxnAborted ();

		private TxnAborted txnAborted;

        public AbortOnFailureReceiveTxnBatch(IBTTransportProxy transportProxy, ControlledTermination control, CommittableTransaction transaction, ManualResetEvent orderedEvent, TxnAborted txnAborted) : base(transportProxy, control, transaction, orderedEvent, false)
        { 
			this.txnAborted = txnAborted;
		}
        public AbortOnFailureReceiveTxnBatch(IBTTransportProxy transportProxy, ControlledTermination control, IDtcTransaction comTxn, CommittableTransaction transaction, ManualResetEvent orderedEvent, TxnAborted txnAborted)
            : base(transportProxy, control, comTxn, transaction, orderedEvent, false)
        {
            this.txnAborted = txnAborted;
        }
        protected override void StartBatchComplete (int hrBatchComplete)
        {
            if (this.HRStatus >= 0)
            {
                this.SetComplete();
            }
        }
        protected override void StartProcessFailures ()
		{
			this.SetAbort();
			if (this.txnAborted != null)
			{
				this.txnAborted();
			}
		}
	}

	//  Anything fails in this batch we abort the lot - even the case where we get an "S_FALSE" because the EPM has handled the error
	public sealed class AbortOnAllFailureReceiveTxnBatch : TxnBatch
	{
		public delegate void StopProcessing ();

		private StopProcessing stopProcessing;

        public AbortOnAllFailureReceiveTxnBatch(IBTTransportProxy transportProxy, ControlledTermination control, CommittableTransaction transaction, ManualResetEvent orderedEvent, StopProcessing stopProcessing) : base(transportProxy, control, transaction, orderedEvent, false)
        {
			this.stopProcessing = stopProcessing;
		}
		protected override void StartBatchComplete (int hrBatchComplete)
		{
            if (this.HRStatus != 0)
            {
                this.SetAbort();
                if (this.stopProcessing != null)
                {
                    this.stopProcessing();
                }
            }
            else
            {
                this.SetComplete();
            }
		}
	}

	//  Submit fails we MoveToSuspendQ
	public sealed class SingleMessageReceiveTxnBatch : TxnBatch
	{
        public SingleMessageReceiveTxnBatch(IBTTransportProxy transportProxy, ControlledTermination control, CommittableTransaction transaction, ManualResetEvent orderedEvent) : base(transportProxy, control, transaction, orderedEvent, true)
        { }
		protected override void StartProcessFailures ()
		{
			if (!this.OverallSuccess)
			{
				this.innerBatch = new AbortOnFailureReceiveTxnBatch(this.TransportProxy, this.control, this.comTxn, this.transaction, this.orderedEvent, null);
				this.innerBatchCount = 0;
			}
		}
		protected override void SubmitFailure (IBaseMessage message, Int32 hrStatus, object userData)
		{
			if (this.innerBatch != null)
			{
                try
                {
                    Stream originalStream = message.BodyPart.GetOriginalDataStream();
				    originalStream.Seek(0, SeekOrigin.Begin);
				    message.BodyPart.Data = originalStream;
                    this.innerBatch.MoveToSuspendQ(message);
					this.innerBatchCount++;
				}
				catch (Exception e)
				{
                    Trace.WriteLine("SingleMessageReceiveTxnBatch.SubmitFailure Exception: {0}", e.Message);
					this.innerBatch = null;
					this.SetAbort();
				}
			}
		}
        protected override void SubmitSuccess(IBaseMessage message, Int32 hrStatus, object userData)
        {
            this.SetComplete();
        }
		protected override void EndProcessFailures ()
		{
			if (this.innerBatch != null && this.innerBatchCount > 0)
			{
				try
				{
					this.innerBatch.Done();
					this.SetPendingWork();
				}
				catch (Exception)
				{
					this.SetAbort();
				}
			}
		}
		private Batch innerBatch;
		private int innerBatchCount;
	}
}

