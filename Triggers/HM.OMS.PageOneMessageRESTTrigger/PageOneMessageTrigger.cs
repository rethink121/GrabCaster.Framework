// PageOneMessageTrigger.cs
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
using System;
using System.Collections.Generic;
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

namespace HM.OMS.PageOneMessageRESTTrigger
{
    /// <summary>
    /// The file trigger.
    /// </summary>
    [TriggerContract("{22F27D62-7D66-4947-9F08-D57E4E5FCC94}", "PageOneMessageTrigger", "PageOneMessageTrigger is the OMS service to send mails", false, true, false)]
    public class PageOneMessageTrigger : ITriggerType, IPageOneMessage
    {
        readonly static AutoResetEvent waitHandle = new AutoResetEvent(false);
        public static PageOneMessageTriggerWebServiceHost engineHost;
        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [TriggerPropertyContract("input", "[message] to send an email - [auth] to check the authentication")]
        public string input { get; set; }

        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [TriggerPropertyContract("From", "The mail from")]
        public string From { get; set; }
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [TriggerPropertyContract("To", "The mail to")]
        public string To { get; set; }
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [TriggerPropertyContract("Message", "Body message")]
        public string Message { get; set; }

        /// <summary>
        /// WebApiEndPoint used by the service
        /// </summary>
        [TriggerPropertyContract("WebApiEndPoint", "WebApiEndPoint used by the service")]
        public string WebApiEndPoint { get; set; }



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


        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="actionTrigger">
        /// The set event action trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [TriggerActionContract("{4C60AEF9-7F37-456D-91E3-179E275F694B}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                //Start Service
                // Start Web API interface
 
                engineHost = new PageOneMessageTriggerWebServiceHost(typeof(PageOneMessageTrigger), new Uri(WebApiEndPoint));
                engineHost.AddServiceEndpoint(typeof(IPageOneMessage), new WebHttpBinding(), ConfigurationBag.EngineName);
                var stp = engineHost.Description.Behaviors.Find<ServiceDebugBehavior>();
                stp.HttpHelpPageEnabled = false;

                input = "/auth";
                engineHost.This = this;
                engineHost.Context = context;
                engineHost.ActionTrigger = actionTrigger;
                engineHost.Open();
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


        public string SendMessage(iPageOneMessage pageOneMessage)
        {
            this.ActionTrigger(this, this.Context);
            //Wait for sync
            waitHandle.WaitOne();
            return EncodingDecoding.EncodingBytes2String(this.DataContext);
        }

        public bool ServiceAvailable()
        {
            return true;
        }

        public string Auth()
        {
            object s = engineHost;
            engineHost.ActionTrigger(engineHost.This, engineHost.Context);
            //Wait for sync
            waitHandle.WaitOne();
            return EncodingDecoding.EncodingBytes2String(engineHost.DataContext);
        }
    }

    public class PageOneMessageTriggerWebServiceHost : WebServiceHost
    {
        public PageOneMessageTriggerWebServiceHost(Type serviceType, params Uri[] baseAddresses):base(serviceType,baseAddresses)
        {
 
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public byte[]DataContext { get; set; }        /// <summary>
                                                      /// Gets or sets the context.
                                                      /// </summary>
        public ITriggerType This { get; set; }
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action trigger.
        /// </summary>
        public ActionTrigger ActionTrigger { get; set; }
    }
}
