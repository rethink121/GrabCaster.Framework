// --------------------------------------------------------------------------------------------------
// <copyright file = "ConfigurationLibrary.cs" company="GrabCaster Ltd">
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

namespace GrabCaster.Framework.Library.Azure
{
    using Microsoft.Azure;

    public enum EventHubsCheckPointPattern
    {
        CheckPoint,

        Dt,

        Dtepoch,

        Dtutcnow,

        Dtnow,

        Dtutcnowepoch,

        Dtnowepoch
    }

    /// <summary>
    /// The configuration.
    /// </summary>
    public static class ConfigurationLibrary
    {


        /// <summary>
        /// The azure name space connection string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AzureNameSpaceConnectionString()
        {
            return CloudConfigurationManager.GetSetting("AzureNameSpaceConnectionString");
        }

        /// <summary>
        /// The group event hubs storage account name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GroupEventHubsStorageAccountName()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsStorageAccountName");
        }

        /// <summary>
        /// The group event hubs storage account key.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GroupEventHubsStorageAccountKey()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsStorageAccountKey");
        }
        /// <summary>
        /// The group event hubs name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GroupEventHubsName()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsName");

        }

        /// <summary>
        /// The engine name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EngineName()
        {
            return "GrabCaster";

        }

        /// <summary>
        /// The point id.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string PointId()
        {
            return CloudConfigurationManager.GetSetting("PointId");
        }

        /// <summary>
        /// The point name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string PointName ()
        {
            return CloudConfigurationManager.GetSetting("PointName");
        }

        /// <summary>
        /// The channel id.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ChannelId()
        {
            return CloudConfigurationManager.GetSetting("ChannelId");
        }

        /// <summary>
        /// The channel name.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ChannelName()
        {
            return CloudConfigurationManager.GetSetting("ChannelName");
        }

    }
}
