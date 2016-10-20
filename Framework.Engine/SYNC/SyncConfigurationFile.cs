// --------------------------------------------------------------------------------------------------
// <copyright file = "SyncConfigurationFile.cs" company="GrabCaster Ltd">
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
    using System;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Base;

    /// <summary>
    /// The sync configuration file.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SyncConfigurationFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncConfigurationFile"/> class.
        /// </summary>
        /// <param name="fileType">
        /// The file type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="fileContent">
        /// The file content.
        /// </param>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        public SyncConfigurationFile(
            string fileType,
            string name,
            byte[] fileContent,
            string channelId)
        {
            this.FileType = fileType;
            this.Name = name;
            this.FileContent = fileContent;
            this.ChannelId = channelId;
        }

        /// <summary>
        /// Gets or sets the file type.
        /// </summary>
        [DataMember]
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the file content.
        /// </summary>
        [DataMember]
        public byte[] FileContent { get; set; }

        /// <summary>
        /// Gets or sets the channel id.
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }
    }
}