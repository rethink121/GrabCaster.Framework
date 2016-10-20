// --------------------------------------------------------------------------------------------------
// <copyright file = "SyncPointMessage.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Contracts.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Contracts.Points;

    /// <summary>
    /// The shell item type.
    /// </summary>
    public enum ShellItemType
    {
        /// <summary>
        /// The host.
        /// </summary>
        Host,

        /// <summary>
        /// The point.
        /// </summary>
        Point,

        /// <summary>
        /// The trigger dll.
        /// </summary>
        TriggerDll,

        /// <summary>
        /// The event dll.
        /// </summary>
        EventDll,

        /// <summary>
        /// The trigger configuration.
        /// </summary>
        TriggerConfiguration,

        /// <summary>
        /// The event configuration.
        /// </summary>
        EventConfiguration
    }

    /// <summary>
    /// The sync point message.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SyncPointMessage
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [DataMember]
        public Host Host { get; set; }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        [DataMember]
        public Point Point { get; set; }

        /// <summary>
        /// Gets or sets the bubbling triggers.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> BubblingTriggers { get; set; }

        /// <summary>
        /// Gets or sets the bubbling events.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> BubblingEvents { get; set; }

        /// <summary>
        /// Gets or sets the triggers.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> DllTriggers { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> DllEvents { get; set; }
    }

    /// <summary>
    /// The host.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Host
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        [DataMember]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}