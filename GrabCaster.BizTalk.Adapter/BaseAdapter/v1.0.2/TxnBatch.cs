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
using System.Runtime.InteropServices;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter.Common
{
	public class TxnBatch : Batch
	{
        public TxnBatch(IBTTransportProxy transportProxy, ControlledTermination control, CommittableTransaction transaction, ManualResetEvent orderedEvent, bool makeSuccessCall) : base(transportProxy, makeSuccessCall)
        {
			this.control = control;

            this.comTxn = TransactionInterop.GetDtcTransaction(transaction);

            //  the System.Transactions transaction - must be the original transaction - only that can be used to commit
            this.transaction = transaction;

			this.orderedEvent = orderedEvent;
		}
        public TxnBatch(IBTTransportProxy transportProxy, ControlledTermination control, IDtcTransaction comTxn, CommittableTransaction transaction, ManualResetEvent orderedEvent, bool makeSuccessCall) : base(transportProxy, makeSuccessCall)
        {
            this.control = control;
            this.comTxn = comTxn;
            this.transaction = transaction;
            this.orderedEvent = orderedEvent;
        }
        public override void Done ()
		{
            this.CommitConfirm = base.Done(this.comTxn);
		}
		protected override void EndBatchComplete ()
		{
			if (this.pendingWork)
			{
				return;
			}
			try
			{
				if (this.needToAbort)
				{
                    this.transaction.Rollback();

                    this.CommitConfirm.DTCCommitConfirm(this.comTxn, false); 
				}
				else
				{
                    this.transaction.Commit();

                    this.CommitConfirm.DTCCommitConfirm(this.comTxn, true); 
				}
			}
			catch
			{
				try
				{
					this.CommitConfirm.DTCCommitConfirm(this.comTxn, false); 
				}
				catch
				{
				}
			}
			//  note the pending work check at the top of this function removes the need to check a needToLeave flag
			this.control.Leave();

			if (this.orderedEvent != null)
				this.orderedEvent.Set();
		}
        protected void SetAbort()
        {
            this.needToAbort = true;
        }
        protected void SetComplete()
        {
            this.needToAbort = false;
        }
        protected void SetPendingWork()
		{
			this.pendingWork = true;
		}
		protected IBTDTCCommitConfirm CommitConfirm
		{
			set
			{
				this.commitConfirm = value;
				this.commitConfirmEvent.Set();
			}
			get
			{
				this.commitConfirmEvent.WaitOne();
				return this.commitConfirm;
			}
		}
        protected IDtcTransaction comTxn;
        protected CommittableTransaction transaction;
        protected ControlledTermination control;
		protected IBTDTCCommitConfirm commitConfirm = null;
		protected ManualResetEvent orderedEvent;
		private ManualResetEvent commitConfirmEvent = new ManualResetEvent(false);
		private bool needToAbort = true;
		private bool pendingWork = false;
	}
}
