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
using System.Threading;
using System.Collections.Generic;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter.Common
{
    public class SyncReceiveSubmitBatch : ReceiveBatch
    {
        private ManualResetEvent workDone;
        private bool overallSuccess = false;
        private ControlledTermination control;

        public SyncReceiveSubmitBatch(IBTTransportProxy transportProxy, ControlledTermination control, int depth)
            : this(transportProxy, control, new ManualResetEvent(false), depth) { }

        private SyncReceiveSubmitBatch(IBTTransportProxy transportProxy, ControlledTermination control,
                                        ManualResetEvent submitComplete, int depth)
            : base(transportProxy, control, submitComplete, depth)
        {
            this.control = control;
            this.workDone = submitComplete;
            base.ReceiveBatchComplete += new ReceiveBatchCompleteHandler(OnBatchComplete);
        }

        private void OnBatchComplete(bool overallSuccess)
        {
            this.overallSuccess = overallSuccess;
        }

        public override void Done()
        {
            bool needToLeave = control.Enter();

            try
            {
                base.Done();
            }
            catch
            {
                if (needToLeave)
                    control.Leave();

                throw;
            }
        }

        public bool Wait()
        {
            this.workDone.WaitOne();

            return this.overallSuccess;
        }
    }
}