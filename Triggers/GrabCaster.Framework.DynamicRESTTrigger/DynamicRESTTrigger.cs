using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;

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
                    Console.WriteLine($"TRGT {guid} - {(DateTime.UtcNow.Second + ":" + DateTime.UtcNow.Millisecond)}");
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    this.DataContext = EncodingDecoding.EncodingString2Bytes(guid);
                    actionTrigger(this, context);
                    WaitHandle.WaitOne();
                    stopwatch.Stop();
                    guidBack = EncodingDecoding.EncodingBytes2String(this.DataContext);
                    Console.WriteLine($"EVTT {guidBack} - {(DateTime.UtcNow.Second + ":" + DateTime.UtcNow.Millisecond)}");

                    long elapsed_time = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine(($"Response in {elapsed_time.ToString()}"));
                    Console.WriteLine(($"0.5 second waitining..."));
                    System.Threading.Thread.Sleep(500);


                }


                Thread.Sleep(Timeout.Infinite);
                return null;

            }
            catch (Exception ex)
            {
                this.DataContext = EncodingDecoding.EncodingString2Bytes(ex.Message);
                actionTrigger(this, context);
                this.ActionTrigger = actionTrigger;
                this.Context = context;
                return EncodingDecoding.EncodingString2Bytes(ex.Message);
            }
        }

        public void SyncAsyncActionReceived(byte[] content)
        {
            string guidBack = EncodingDecoding.EncodingBytes2String(content);
            Console.WriteLine($"EVTE {guidBack} - {(DateTime.UtcNow.Second + ":" + DateTime.UtcNow.Millisecond)}");

            WaitHandle.Set();

        }
    }
}
