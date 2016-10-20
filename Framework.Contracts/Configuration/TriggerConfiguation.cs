// --------------------------------------------------------------------------------------------------
// <copyright file = "TriggerConfiguation.cs" company="GrabCaster Ltd">
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

using GrabCaster.Framework.Contracts.Channels;

namespace GrabCaster.Framework.Contracts.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Base;

    using Newtonsoft.Json;

    /// <summary>
    ///     Trigger configuration File, To create a configuration file trigger who able to activate a trigger action/s
    /// </summary>
    [DataContract]
    [Serializable]
    public class TriggerConfiguration
    {
        /// <summary>
        /// Gets or sets the trigger.
        /// </summary>
        [DataMember]
        public Trigger Trigger { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<Event> Events { get; set; }


    }

    /// <summary>
    /// The trigger.
    /// </summary>
    [DataContract,Serializable]
    public class Trigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trigger"/> class.
        /// </summary>
        /// <param name="idComponent">
        /// The id component.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public Trigger(string idComponent, string idConfiguration, string name, string description)
        {
            this.IdComponent = idComponent;
            this.IdConfiguration = idConfiguration;
            this.Name = name;
            this.Description = description;
        }


        /// <summary>
        /// Gets or sets the id component.
        /// </summary>
        [DataMember]
        public string IdComponent { get; set; }

        /// <summary>
        /// Get or set the id configuration
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
        /// Gets or sets the channels where the trigger could be executes, if null then can be executed anyway.
        /// </summary>
        [DataMember]
        public List<Channel> Channels { get; set; }
        /// <summary>
        /// Gets or sets the id chains.
        /// </summary>
        [DataMember]
        public List<Chain> Chains { get; set; }

        /// <summary>
        /// Gets or sets the trigger properties.
        /// </summary>
        [DataMember]
        public List<TriggerProperty> TriggerProperties { get; set; }
    }

    /// <summary>
    /// The trigger property.
    /// </summary>
    [DataContract, Serializable]
    public class TriggerProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerProperty"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public TriggerProperty(string name, object value)
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
    /// The trigger action.
    /// </summary>
    [DataContract, Serializable]
    public class TriggerAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerAction"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public TriggerAction(string id, string name)
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