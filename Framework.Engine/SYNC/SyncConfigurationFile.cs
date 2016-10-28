// SyncConfigurationFile.cs
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

namespace GrabCaster.Framework.Engine
{
    using System;
    using System.Runtime.Serialization;

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
            FileType = fileType;
            Name = name;
            FileContent = fileContent;
            ChannelId = channelId;
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