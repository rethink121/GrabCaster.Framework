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

namespace GrabCaster.Framework.BizTalk.Adapter.Common
{
    public class ControlledTermination : IDisposable
    {
        private AutoResetEvent e = new AutoResetEvent(false);
        private int activityCount = 0;
        private bool terminate = false;

        //  to be called at the start of the activity
        //  returns false if terminate has been called
        public bool Enter ()
        {
            lock (this)
            {
                if (true == this.terminate)
                {
                    return false;
                }

                this.activityCount++;
            }
            return true;
        }

        //  to be called at the end of the activity
        public void Leave ()
        {
            lock (this)
            {
                this.activityCount--;

                // Set the event only if Terminate() is called
                if (this.activityCount == 0 && this.terminate)
                    this.e.Set();
            }
        }

        //  this method blocks waiting for any activity to complete
        public void Terminate ()
        {
            bool result;

            lock (this)
            {
                this.terminate = true;
                result = (this.activityCount == 0);
            }

            // If activity count was not zero, wait for pending activities
            if (!result)
            {
                this.e.WaitOne();
            }
        }

        public bool TerminateCalled
        {
            get
            { 
                lock (this)
                {
                    return this.terminate;
                }
            }
        }

        public void Dispose()
        {
            ((IDisposable)this.e).Dispose();
        }
    }
}