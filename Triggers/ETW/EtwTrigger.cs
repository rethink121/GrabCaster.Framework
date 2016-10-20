// --------------------------------------------------------------------------------------------------
// <copyright file = "EtwTrigger.cs" company="GrabCaster Ltd">
//   Copyright (c) 2013 - 2016 GrabCaster Ltd All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
// 
//    Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.ETW
{
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

    /// <summary>
    /// The etw trigger.
    /// </summary>
    [TriggerContract("{753B071D-FD3D-443F-8368-0727CA8BE84E}", "ETW Trigger", "Intercept ETW Message", false, true,
        false)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class EtwTrigger : ITriggerType
    {
        /// <summary>
        /// The lock slim eh queue.
        /// </summary>
        public static LockSlimEhQueue<byte[]> LockSlimEhQueue { get; } = null;

        [TriggerPropertyContract("EtwProvider", "Event Source to monitor")]
        public string EtwProvider { get; set; }

        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }

        public string SupportBag { get; set; }
        public ActionContext Context { get; set; }

        public ActionTrigger ActionTrigger { get; set; }

        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        [TriggerActionContract("{C7D6B7CC-7F65-4616-8902-72680148A57B}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                this.ActionTrigger = actionTrigger;
                this.Context = context;

                var sprovider = this.EtwProvider;
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
        protected int CaptureLimit;

        protected EtwTrigger EtwTrigger;

        protected ActionContext ActionContext;

        protected ReaderWriterLockSlim Locker;

        protected int OnPublishExecuted;

        //EH
        protected ActionTrigger ActionTrigger;

        protected int TimeLimit;

        public Timer InternalTimer;

        public LockSlimEhQueue(
            int capLimit,
            int timeLimit,
            ActionTrigger actionTrigger,
            ActionContext actionContext,
            EtwTrigger eTwTrigger)
        {
            this.Init(capLimit, timeLimit);
            this.EtwTrigger = eTwTrigger;
            this.ActionTrigger = actionTrigger;
            this.ActionContext = actionContext;
        }

        public event Action<List<T>> OnPublish = delegate { };

        //Write the item
        public new virtual void Enqueue(T item)
        {
            //put in queue
            base.Enqueue(item);
            //If > caplimit the publish
            if (this.Count >= this.CaptureLimit)
            {
                this.Publish();
            }
        }

        private void Init(int capLimit, int timeLimit)
        {
            this.CaptureLimit = capLimit;
            this.TimeLimit = timeLimit;
            this.Locker = new ReaderWriterLockSlim();
            this.InitTimer();
        }

        protected virtual void InitTimer()
        {
            this.InternalTimer = new Timer { AutoReset = false, Interval = this.TimeLimit * 1000 };
            this.InternalTimer.Elapsed += (s, e) => { this.Publish(); };
            this.InternalTimer.Start();
        }

        protected virtual void Publish()
        {
            var task = new Task(
                () =>
                    {
                        var itemsToLog = new List<T>();
                        try
                        {
                            if (Interlocked.CompareExchange(ref this.OnPublishExecuted, 1, 0) > 0)
                            {
                                return;
                            }

                            //if (IsPublishing())
                            //    return;

                            this.InternalTimer.Stop();

                            T item;
                            while (this.TryDequeue(out item))
                            {
                                itemsToLog.Add(item);
                                this.ActionTrigger(this.EtwTrigger, this.ActionContext);
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
                            Interlocked.Decrement(ref this.OnPublishExecuted);
                            this.OnPublish(itemsToLog);
                            this.CompletePublishing();
                        }
                    });
            task.Start();
        }

        public bool IsPublishing()
        {
            return (Interlocked.CompareExchange(ref this.OnPublishExecuted, 1, 0) > 0);
        }

        private void CompletePublishing()
        {
            this.InternalTimer.Start();
            Interlocked.Decrement(ref this.OnPublishExecuted);
        }
    }
}