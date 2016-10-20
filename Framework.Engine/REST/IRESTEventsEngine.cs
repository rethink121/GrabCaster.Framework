// --------------------------------------------------------------------------------------------------
// <copyright file = "IRESTEventsEngine.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Engine
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using GrabCaster.Framework.Base;

    /// <summary>
    /// The RestEventsEngine interface.
    /// </summary>
    [ServiceContract]
    public interface IRestEventsEngine
    {
        /// <summary>
        /// The sync send bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
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
        /// The sync send bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendBubblingConfiguration(string channelId, string pointId);

        /// <summary>
        /// The sync send file bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="messageType">
        /// The message type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendFileBubblingConfiguration(
            string channelId,
            string pointId,
            string fileName,
            string messageType);

        /// <summary>
        /// The sync send request bubbling configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendRequestBubblingConfiguration(string channelId, string pointId);

        /// <summary>
        /// The sync send component.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        //[OperationContract]
        //[WebGet]
        //string SyncSendComponent(string channelId, string pointId, string idComponent);

        /// <summary>
        /// The sync send request configuration.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendRequestConfiguration(string channelId, string pointId);

        /// <summary>
        /// The sync send request component.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="pointId">
        /// The point id.
        /// </param>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string SyncSendRequestComponent(string channelId, string pointId, string idComponent);

        /// <summary>
        /// The configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        Stream Configuration();

        /// <summary>
        /// The execute trigger.
        /// </summary>
        /// <param name="triggerId">
        /// The trigger id.
        /// </param>
        /// <param name="configurationId"></param>
        /// <param name="value"></param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string ExecuteTrigger(string configurationId, string triggerId, string value);

        /// <summary>
        /// The refresh bubbling setting.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [OperationContract]
        [WebGet]
        string RefreshBubblingSetting();
    }
}