// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsDownStream.cs" company="GrabCaster Ltd">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.Bubbling;

namespace GrabCaster.Framework.Dcp.Redis
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Log;

    using Microsoft.ServiceBus.Messaging;

    using StackExchange.Redis;

    [EventsOnRampContract("{377B04BD-C80C-4AC5-BC70-C5CC571B2BDC}", "EventsDownStream", "Redis EventsDownStream")]
    public class OnRampStream: IOnRampStream
    {
        public void Run(SetEventOnRampMessageReceived setEventOnRampMessageReceived)
        {

            try
            {

                var myNewThread = new Thread(() => this.StartRedisListener(setEventOnRampMessageReceived));
                myNewThread.Start();
            }
            catch (Exception ex)
            {

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    Constant.LogLevelError);
            }
        }

        public void StartRedisListener(SetEventOnRampMessageReceived setEventOnRampMessageReceived)
        {

            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationBag.Configuration.RedisConnectionString);

                ISubscriber sub = redis.GetSubscriber();

                sub.Subscribe("*", (channel, message) => {
                    byte[] byteArray = (byte[])message;
                    BubblingObject bubblingObject = BubblingObject.DeserializeMessage(byteArray);
                    setEventOnRampMessageReceived(bubblingObject);
                });
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesEventHubs,
                    ex,
                    Constant.LogLevelError);
            }
        }
    }
}
