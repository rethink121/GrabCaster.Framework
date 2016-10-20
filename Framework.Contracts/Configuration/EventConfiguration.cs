// --------------------------------------------------------------------------------------------------
// <copyright file = "EventConfiguration.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Contracts.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Channels;

    /// <summary>
    ///     Trigger event File
    /// </summary>
    [DataContract]
    [Serializable]
    public class EventConfiguration
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        [DataMember]
        public Event Event { get; set; }
    }

    /// <summary>
    /// The event.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <param name="idConfiguration">
        /// The id configuration.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public Event(string idComponent, string idConfiguration, string name, string description)
        {
            this.IdConfiguration = idConfiguration;
            this.IdComponent = idComponent;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the id component.
        /// </summary>
        [DataMember]
        public string IdComponent { get; set; }

        /// <summary>
        /// Gets or sets the id configuration.
        /// </summary>
        [DataMember]
        public string IdConfiguration { get; set; }

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
        /// Gets or sets the id chains.
        /// </summary>
        [DataMember]
        public List<Chain> Chains { get; set; }

        /// <summary>
        /// Gets or sets the event properties.
        /// </summary>
        [DataMember]
        public List<EventProperty> EventProperties { get; set; }

        /// <summary>
        /// Cache in dictionary the event properties.
        /// </summary>
        public Dictionary<string,EventProperty> CacheEventProperties { get; set; }

        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        [DataMember]
        public List<Channel> Channels { get; set; }

        /// <summary>
        /// Gets or sets the correlation.
        /// </summary>
        [DataMember]
        public Correlation Correlation { get; set; }
    }

    /// <summary>
    /// The event property.
    /// </summary>
    [DataContract]
    [Serializable]
    public class EventProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventProperty"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public EventProperty(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        ///     Property name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [DataMember]
        public object Value { get; set; }
    }

    /// <summary>
    /// The event action.
    /// </summary>
    [DataContract]
    [Serializable]
    public class EventAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAction"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public EventAction(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}