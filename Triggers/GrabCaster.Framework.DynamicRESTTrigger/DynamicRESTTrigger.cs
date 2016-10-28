// DynamicRESTTrigger.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
//   - Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//   - Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
//   
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;
using System;
using System.Diagnostics;
using System.Threading;

namespace GrabCaster.Framework.DynamicRESTTrigger
{
    /// <summary>
    /// The file trigger.
    /// </summary>
    [TriggerContract("{95C6319B-35B6-4AB0-88F8-49A6E332D270}", "PageOneMessageTrigger",
         "PageOneMessageTrigger is the OMS service to send mails", false, true, false)]
    public class DynamicRESTTrigger : ITriggerType
    {
        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }

        /// <summary>
        /// WebApiEndPoint used by the service
        /// </summary>
        [TriggerPropertyContract("WebApiEndPoint", "WebApiEndPoint used by the service")]
        public string WebApiEndPoint { get; set; }

        [TriggerPropertyContract("Behaviour", "Behaviour to drive the connectivity type")]
        public string Behaviour { get; set; }

        public AutoResetEvent WaitHandle { get; set; }

        public string SupportBag { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action trigger.
        /// </summary>
        public ActionTrigger ActionTrigger { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        //questo puoi metterlo in interfaccia
        public delegate byte[] SetGetDataTrigger(
            ActionTrigger actionTrigger, ActionContext context);


        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="actionTrigger">
        /// The set event action trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [TriggerActionContract("{62DD5BB1-C27B-4341-A277-FE7023775AC3}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                //setGetDataTrigger = GetDataTrigger;
                //DynamicRESTService.StartService(WebApiEndPoint);
                string guid = string.Empty;
                string guidBack = string.Empty;
                //EventBehaviour eventBehaviour = (EventBehaviour)Enum.Parse(typeof(EventBehaviour), Behaviour);
                WaitHandle = new AutoResetEvent(false);
                while (true)
                {
                    guid = Guid.NewGuid().ToString();
                    //context.EventBehaviour = eventBehaviour;
                    Console.WriteLine($"TRGT {guid} - {DateTime.UtcNow.Second + ":" + DateTime.UtcNow.Millisecond}");
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    DataContext = EncodingDecoding.EncodingString2Bytes(guid);
                    actionTrigger(this, context);
                    WaitHandle.WaitOne();
                    stopwatch.Stop();
                    guidBack = EncodingDecoding.EncodingBytes2String(DataContext);
                    Console.WriteLine($"EVTT {guidBack} - {DateTime.UtcNow.Second + ":" + DateTime.UtcNow.Millisecond}");

                    long elapsed_time = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine($"Response in {elapsed_time}");
                    Console.WriteLine($"0.5 second waitining...");
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                DataContext = EncodingDecoding.EncodingString2Bytes(ex.Message);
                actionTrigger(this, context);
                ActionTrigger = actionTrigger;
                Context = context;
                return EncodingDecoding.EncodingString2Bytes(ex.Message);
            }
        }

        public void SyncAsyncActionReceived(byte[] content)
        {
            string guidBack = EncodingDecoding.EncodingBytes2String(content);
            Console.WriteLine($"EVTE {guidBack} - {DateTime.UtcNow.Second + ":" + DateTime.UtcNow.Millisecond}");

            WaitHandle.Set();
        }
    }
}