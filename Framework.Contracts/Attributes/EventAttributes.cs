// --------------------------------------------------------------------------------------------------
// <copyright file = "EventAttributes.cs" company="GrabCaster Ltd">
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

    /// <summary>
    /// The event contract.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class EventContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventContract"/> class.
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
        /// <param name="shared">
        /// The shared.
        /// </param>
        public EventContract(string id, string name, string description, bool shared)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Shared = shared;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared.
        /// </summary>
        public bool Shared { get; set; }
    }

    /// <summary>
    /// The event property contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EventPropertyContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventPropertyContract"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public EventPropertyContract(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        ///     Method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// The event action contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EventActionContract : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventActionContract"/> class.
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
        public EventActionContract(string id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}