// --------------------------------------------------------------------------------------------------
// <copyright file = "TriggerAttributes.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Contracts.Attributes
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The trigger contract.
    /// </summary>
    [DataContract]
    [Serializable]

    [AttributeUsage(AttributeTargets.Class)] // Multiuse attribute.
    public class TriggerContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerContract"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="pollingRequired">
        /// The polling required.
        /// </param>
        /// <param name="shared">
        /// The shared.
        /// </param>
        /// <param name="nop">
        /// The no operation.
        /// </param>
        public TriggerContract(string id, string name, string description, bool pollingRequired, bool shared, bool nop)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Shared = shared;
            this.PollingRequired = pollingRequired;
            this.Nop = nop;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

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
        /// Gets or sets a value indicating whether no operation.
        /// </summary>
        [DataMember]
        public bool Nop { get; set; }
    }

    /// <summary>
    /// The trigger property contract.
    /// </summary>
    [DataContract]
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class TriggerPropertyContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerPropertyContract"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public TriggerPropertyContract(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        ///     Property name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }

    /// <summary>
    /// The trigger action contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [DataContract]
    [Serializable]
    public class TriggerActionContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerActionContract"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public TriggerActionContract(string id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
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

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}