// --------------------------------------------------------------------------------------------------
// <copyright file = "Channel.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Contracts.Channels
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Contracts.Points;

    /// <summary>
    /// The channel.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Channel : IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="channelId">
        /// The channel id.
        /// </param>
        /// <param name="channelName">
        /// The channel name.
        /// </param>
        /// <param name="channelDescription">
        /// The channel description.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        public Channel(string channelId, string channelName, string channelDescription, List<Point> points)
        {
            this.ChannelId = channelId;
            this.ChannelName = channelName;
            this.ChannelDescription = channelDescription;
            this.Points = points;
        }

        /// <summary>
        /// Gets or sets the channel id.
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the channel name.
        /// </summary>
        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        /// Gets or sets the channel description.
        /// </summary>
        [DataMember]
        public string ChannelDescription { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        [DataMember]
        public List<Point> Points { get; set; }
    }
}