// LockSlimQueueEngine.cs
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
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Log;
using Timer = System.Timers.Timer;

#endregion

namespace GrabCaster.Framework.Engine
{
    public abstract class LockSlimQueueEngine<BubblingObject> : ConcurrentQueue<BubblingObject>
        where BubblingObject : class
    {
        /// <summary>
        ///     The cap limit.
        /// </summary>
        protected int CapLimit;

        /// <summary>
        ///     The internal timer.
        /// </summary>
        protected Timer InternalTimer;

        /// <summary>
        ///     The locker.
        /// </summary>
        protected ReaderWriterLockSlim Locker;

        /// <summary>
        ///     The on publish executed.
        /// </summary>
        protected int OnPublishExecuted;

        /// <summary>
        ///     The time limit.
        /// </summary>
        protected int TimeLimit;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockSlimQueueEngine{T}" /> class.
        /// </summary>
        protected LockSlimQueueEngine()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockSlimQueueEngine{T}" /> class.
        /// </summary>
        /// <param name="capLimit">
        ///     TODO The cap limit.
        /// </param>
        /// <param name="timeLimit">
        ///     TODO The time limit.
        /// </param>
        protected LockSlimQueueEngine(int capLimit, int timeLimit)
        {
            Init(capLimit, timeLimit);
        }

        /// <summary>
        ///     TODO The on publish.
        /// </summary>
        public event Action<List<BubblingObject>> OnPublish = delegate { };

        /// <summary>
        ///     TODO The enqueue.
        /// </summary>
        /// <param name="item">
        ///     TODO The item.
        /// </param>
        public new virtual void Enqueue(BubblingObject item)
        {
            base.Enqueue(item);
            if (Count >= CapLimit)
            {
                Debug.WriteLine($"CapLimit!: {CapLimit} > Publish!");
                Publish();
            }
        }

        /// <summary>
        ///     TODO The init.
        /// </summary>
        /// <param name="capLimit">
        ///     TODO The cap limit.
        /// </param>
        /// <param name="timeLimit">
        ///     TODO The time limit.
        /// </param>
        private void Init(int capLimit, int timeLimit)
        {
            CapLimit = capLimit;
            TimeLimit = timeLimit;
            Locker = new ReaderWriterLockSlim();
            InitTimer();
        }

        /// <summary>
        ///     TODO The init timer.
        /// </summary>
        protected virtual void InitTimer()
        {
            InternalTimer = new Timer {AutoReset = false, Interval = TimeLimit*1000};
            InternalTimer.Elapsed += (s, e) =>
            {
                Debug.WriteLine($"TimeLimit!: {TimeLimit} > Publish!");
                Publish();
            };
            InternalTimer.Start();
        }

        /// <summary>
        ///     TODO The publish.
        /// </summary>
        protected virtual void Publish()
        {
            var task = new Task(
                () =>
                {
                    var itemsToPublish = new List<BubblingObject>();
                    try
                    {
                        if (IsPublishing())
                        {
                            return;
                        }

                        StartPublishing();
                        //Debug.WriteLine($"Log start dequeue {this.Count} items!");
                        BubblingObject item;
                        while (TryDequeue(out item))
                        {
                            itemsToPublish.Add(item);
                        }
                    }
                    catch (ThreadAbortException tex)
                    {
                        Debug.WriteLine($"Dequeue items failed > {tex.Message}");

                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Error in {MethodBase.GetCurrentMethod().Name}",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            tex,
                            Constant.LogLevelError);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Dequeue items failed > {ex.Message}");

                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Error in {MethodBase.GetCurrentMethod().Name}",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            ex,
                            Constant.LogLevelError);
                    }
                    finally
                    {
                        //Debug.WriteLine($"Log dequeued {itemsToLog.Count} items");
                        OnPublish(itemsToPublish);
                        CompletePublishing();
                    }
                });
            task.Start();
        }

        /// <summary>
        ///     TODO The is publishing.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        private bool IsPublishing()
        {
            return Interlocked.CompareExchange(ref OnPublishExecuted, 1, 0) > 0;
        }

        /// <summary>
        ///     TODO The start publishing.
        /// </summary>
        private void StartPublishing()
        {
            InternalTimer.Stop();
        }

        /// <summary>
        ///     TODO The complete publishing.
        /// </summary>
        private void CompletePublishing()
        {
            InternalTimer.Start();
            Interlocked.Decrement(ref OnPublishExecuted);
        }
    }
}