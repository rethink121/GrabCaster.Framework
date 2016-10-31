// ConfigurationLibrary.cs
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

using Microsoft.Azure;

#endregion

namespace GrabCaster.Framework.Library.Azure
{
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
    ///     The configuration.
    /// </summary>
    public static class ConfigurationLibrary
    {
        /// <summary>
        ///     The azure name space connection string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string AzureNameSpaceConnectionString()
        {
            return CloudConfigurationManager.GetSetting("AzureNameSpaceConnectionString");
        }

        /// <summary>
        ///     The group event hubs storage account name.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GroupEventHubsStorageAccountName()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsStorageAccountName");
        }

        /// <summary>
        ///     The group event hubs storage account key.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GroupEventHubsStorageAccountKey()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsStorageAccountKey");
        }

        /// <summary>
        ///     The group event hubs name.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GroupEventHubsName()
        {
            return CloudConfigurationManager.GetSetting("GroupEventHubsName");
        }

        /// <summary>
        ///     The engine name.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string EngineName()
        {
            return "GrabCaster";
        }

        /// <summary>
        ///     The point id.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string PointId()
        {
            return CloudConfigurationManager.GetSetting("PointId");
        }

        /// <summary>
        ///     The point name.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string PointName()
        {
            return CloudConfigurationManager.GetSetting("PointName");
        }

        /// <summary>
        ///     The channel id.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string ChannelId()
        {
            return CloudConfigurationManager.GetSetting("ChannelId");
        }

        /// <summary>
        ///     The channel name.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string ChannelName()
        {
            return CloudConfigurationManager.GetSetting("ChannelName");
        }
    }
}