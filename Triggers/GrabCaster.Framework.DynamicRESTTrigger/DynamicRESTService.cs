﻿// DynamicRESTService.cs
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
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
#region Usings

using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Globals;

#endregion

namespace GrabCaster.Framework.DynamicRESTTrigger
{
    public class DynamicRestService : IDynamicRest
    {
        private static Func<ActionTrigger, ActionContext> _getDataTrigger;

        static WebServiceHost engineHost;

        public string GetValue(string fromValue)
        {
            //execute the trigger event
            //witfor
            // RunTheMethod GetDataTrigger();
            return null;
        }

        public static bool StartService(string webApiEndPoint, Func<ActionTrigger, ActionContext> getDataTrigger)
        {
            _getDataTrigger = getDataTrigger;

            engineHost = new WebServiceHost(typeof(DynamicRestService), new Uri("http://localhost:8000"));
            engineHost.AddServiceEndpoint(typeof(DynamicRestService), new WebHttpBinding(), ConfigurationBag.EngineName);
            var stp = engineHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = false;
            engineHost.Open();
            Thread.Sleep(Timeout.Infinite);
            return true;
        }
    }
}