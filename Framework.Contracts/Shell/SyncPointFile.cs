// --------------------------------------------------------------------------------------------------
// <copyright file = "SyncPointFile.cs" company="GrabCaster Ltd">
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
    using System.Runtime.Serialization;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// The sync point file.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SyncPointFile
    {
        /// <summary>
        /// Gets or sets the bubbling event type.
        /// </summary>
        [DataMember]
        public BubblingEventType BubblingEventType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        [DataMember]
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared.
        /// </summary>
        [DataMember]
        public bool Shared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether polling required.
        /// </summary>
        [DataMember]
        public bool PollingRequired { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        [DataMember]
        public List<Property> Properties { get; set; }

        /// <summary>
        /// Gets or sets the base actions.
        /// </summary>
        [DataMember]
        public List<BaseAction> BaseActions { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<Event> Events { get; set; }
    }
}