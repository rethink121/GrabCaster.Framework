﻿// Channel.cs
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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GrabCaster.Framework.Contracts.Points;

#endregion

namespace GrabCaster.Framework.Contracts.Channels
{
    /// <summary>
    ///     The channel.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Channel : IChannel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Channel" /> class.
        /// </summary>
        /// <param name="channelId">
        ///     The channel id.
        /// </param>
        /// <param name="channelName">
        ///     The channel name.
        /// </param>
        /// <param name="channelDescription">
        ///     The channel description.
        /// </param>
        /// <param name="points">
        ///     The points.
        /// </param>
        public Channel(string channelId, string channelName, string channelDescription, List<Point> points)
        {
            ChannelId = channelId;
            ChannelName = channelName;
            ChannelDescription = channelDescription;
            Points = points;
        }

        /// <summary>
        ///     Gets or sets the channel id.
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        ///     Gets or sets the channel name.
        /// </summary>
        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        ///     Gets or sets the channel description.
        /// </summary>
        [DataMember]
        public string ChannelDescription { get; set; }

        /// <summary>
        ///     Gets or sets the points.
        /// </summary>
        [DataMember]
        public List<Point> Points { get; set; }
    }
}