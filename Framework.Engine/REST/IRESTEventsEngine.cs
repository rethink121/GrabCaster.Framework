﻿// IRESTEventsEngine.cs
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

using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

#endregion

namespace GrabCaster.Framework.Engine
{
    /// <summary>
    ///     The RestEventsEngine interface.
    /// </summary>
    [ServiceContract]
    public interface IRestEventsEngine
    {
        /// <summary>
        ///     The sync send bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        ///     The channel id.
        /// </param>
        /// <param name="pointId">
        ///     The point id.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        [OperationContract]
        [WebGet]
        string Deploy(string configuration, string platform);

        [OperationContract]
        [WebGet]
        string SyncPush(string channelId, string pointId);

        [OperationContract]
        [WebGet]
        string SyncPull(string channelId, string pointId);

        [OperationContract]
        [WebGet]
        string Sync();

        /// <summary>
        ///     The configuration.
        /// </summary>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        [OperationContract]
        [WebGet]
        Stream Configuration();

        /// <summary>
        ///     The execute trigger.
        /// </summary>
        /// <param name="triggerId">
        ///     The trigger id.
        /// </param>
        /// <param name="configurationId"></param>
        /// <param name="value"></param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        [OperationContract]
        [WebGet]
        string ExecuteTrigger(string configurationId, string triggerId, string value);

        /// <summary>
        ///     The refresh bubbling setting.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        [OperationContract]
        [WebGet]
        string RefreshBubblingSetting();
    }
}