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

namespace GrabCaster.Framework.Dcp.Azure
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Log;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    ///     Send messages to EH
    /// </summary>
    [EventsOffRampContract("{6FAEA018-C21B-423E-B860-3F8BAC0BC637}", "EventUpStream", "Event Hubs EventUpStream")]
    public class OffRampStream: IOffRampStream
    {
        //EH variable

        private static string connectionString = "";

        private static string eventHubName = "";

        private static EventHubClient eventHubClient;
        
        public bool CreateOffRampStream()
        {
            try
            {
                //EH Configuration
                connectionString = ConfigurationBag.Configuration.AzureNameSpaceConnectionString;
                eventHubName = ConfigurationBag.Configuration.GroupEventHubsName;

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Event Hubs transfort Type: {ConfigurationBag.Configuration.ServiceBusConnectivityMode}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    null,
                    Constant.LogLevelInformation);

                var builder = new ServiceBusConnectionStringBuilder(connectionString)
                                  {
                                      TransportType =
                                          TransportType.Amqp
                                  };

                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Send a EventMessage message
        ///     invio importantissimo perche spedisce eventi e oggetti in array bytela dimensione e strategica
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(BubblingObject message)
        {
            try
            {
                byte[] byteArrayBytes = BubblingObject.SerializeMessage(message);
                EventData evtData = new EventData(byteArrayBytes);
                eventHubClient.Send(evtData);
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