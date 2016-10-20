// --------------------------------------------------------------------------------------------------
// <copyright file = "EventsUpStream.cs" company="GrabCaster Ltd">
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

using GrabCaster.Framework.Contracts.Bubbling;

namespace GrabCaster.Framework.Dcp.Redis
{
    using System.Diagnostics;
    using System.Reflection;
    using System;
    using System.IO;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Log;

    using Microsoft.ServiceBus.Messaging;

    using StackExchange.Redis;

    [EventsOffRampContract("{A51FA36B-7778-47A1-B6DF-5CEC4B8F36B1}", "EventUpStream", "Redis EventUpStream")]
    class OffRampStream : IOffRampStream
    {
        private ISubscriber subscriber;
        public bool CreateOffRampStream()
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationBag.Configuration.RedisConnectionString);
                this.subscriber = redis.GetSubscriber();
                return true;
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
                return false;
            }

        }

        public void SendMessage(BubblingObject message)
        {
            byte[] byteArrayBytes = BubblingObject.SerializeMessage(message);
            this.subscriber.Publish("*", byteArrayBytes);
        }
    }
}
