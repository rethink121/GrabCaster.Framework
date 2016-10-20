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
    ///     Component event File
    /// </summary>
    [DataContract]
    [Serializable]
    public class ComponentConfiguration
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        [DataMember]
        public Component Component { get; set; }
    }

    /// <summary>
    /// The component.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Component
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
        public Component(string idComponent, string name, string description)
        {
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
        /// Gets or sets the event properties.
        /// </summary>
        [DataMember]
        public List<ComponentProperty> ComponentProperties { get; set; }

    }

    /// <summary>
    /// The event property.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ComponentProperty
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
        public ComponentProperty(string name, object value)
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


}