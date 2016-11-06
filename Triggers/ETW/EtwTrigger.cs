// EtwTrigger.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;
using Timer = System.Timers.Timer;

#endregion

namespace GrabCaster.Framework.ETW
{
    /// <summary>
    ///     The etw trigger.
    /// </summary>
    [TriggerContract("{753B071D-FD3D-443F-8368-0727CA8BE84E}", "ETW Trigger", "Intercept ETW Message", false, true,
         false)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
         Justification = "Reviewed. Suppression is OK here.")]
    public class EtwTrigger : ITriggerType
    {
        /// <summary>
        ///     The lock slim eh queue.
        /// </summary>
        public static LockSlimEhQueue<byte[]> LockSlimEhQueue { get; } = null;

        [TriggerPropertyContract("EtwProvider", "Event Source to monitor")]
        public string EtwProvider { get; set; }

        public string SupportBag { get; set; }

        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }

        public ActionContext Context { get; set; }

        public ActionTrigger ActionTrigger { get; set; }

        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        [TriggerActionContract("{C7D6B7CC-7F65-4616-8902-72680148A57B}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                ActionTrigger = actionTrigger;
                Context = context;

                var sprovider = EtwProvider;
                var rewriteProviderId = new Guid("13D5F7EF-9404-47ea-AF13-85484F09F2A7");
                //lockSlimEHQueue = new LockSlimEHQueue<byte[]>(1, 1, SetEventActionTrigger, context, this);
                using (var watcher = new EventTraceWatcher(sprovider, rewriteProviderId))
                {
                    watcher.EventArrived += delegate
                    {
                        //DataContext = EncodingDecoding.EncodingString2Bytes(e.Properties["EventData"].ToString());
                        //lockSlimEHQueue.Enqueue(DataContext);
                        actionTrigger(this, context);
                    };

                    // Start listening
                    watcher.Start();

                    Thread.Sleep(Timeout.Infinite);
                }
                return null;
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }
    }

    public class LockSlimEhQueue<T> : ConcurrentQueue<T>
        where T : class
    {
        protected ActionContext ActionContext;

        //EH
        protected ActionTrigger ActionTrigger;
        protected int CaptureLimit;

        protected EtwTrigger EtwTrigger;

        public Timer InternalTimer;

        protected ReaderWriterLockSlim Locker;

        protected int OnPublishExecuted;

        protected int TimeLimit;

        public LockSlimEhQueue(
            int capLimit,
            int timeLimit,
            ActionTrigger actionTrigger,
            ActionContext actionContext,
            EtwTrigger eTwTrigger)
        {
            Init(capLimit, timeLimit);
            EtwTrigger = eTwTrigger;
            ActionTrigger = actionTrigger;
            ActionContext = actionContext;
        }

        public event Action<List<T>> OnPublish = delegate { };

        //Write the item
        public new virtual void Enqueue(T item)
        {
            //put in queue
            base.Enqueue(item);
            //If > caplimit the publish
            if (Count >= CaptureLimit)
            {
                Publish();
            }
        }

        private void Init(int capLimit, int timeLimit)
        {
            CaptureLimit = capLimit;
            TimeLimit = timeLimit;
            Locker = new ReaderWriterLockSlim();
            InitTimer();
        }

        protected virtual void InitTimer()
        {
            InternalTimer = new Timer {AutoReset = false, Interval = TimeLimit*1000};
            InternalTimer.Elapsed += (s, e) => { Publish(); };
            InternalTimer.Start();
        }

        protected virtual void Publish()
        {
            var task = new Task(
                () =>
                {
                    var itemsToLog = new List<T>();
                    try
                    {
                        if (Interlocked.CompareExchange(ref OnPublishExecuted, 1, 0) > 0)
                        {
                            return;
                        }

                        //if (IsPublishing())
                        //    return;

                        InternalTimer.Stop();

                        T item;
                        while (TryDequeue(out item))
                        {
                            itemsToLog.Add(item);
                            ActionTrigger(EtwTrigger, ActionContext);
                        }
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    finally
                    {
                        Interlocked.Decrement(ref OnPublishExecuted);
                        OnPublish(itemsToLog);
                        CompletePublishing();
                    }
                });
            task.Start();
        }

        public bool IsPublishing()
        {
            return Interlocked.CompareExchange(ref OnPublishExecuted, 1, 0) > 0;
        }

        private void CompletePublishing()
        {
            InternalTimer.Start();
            Interlocked.Decrement(ref OnPublishExecuted);
        }
    }
}