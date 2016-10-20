// --------------------------------------------------------------------------------------------------
// <copyright file = "Property.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Contracts.Bubbling
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    [DataContract]
    [Serializable]
    public class Property : IProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="assemblyPropertyInfo">
        /// The assembly property info.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public Property(string name, string description, PropertyInfo assemblyPropertyInfo, Type type, object value)
        {
            this.Name = name;
            this.Description = description;
            this.AssemblyPropertyInfo = assemblyPropertyInfo;
            this.Type = type;
            this.Value = value;
        }

        /// <summary>
        /// Description of method
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Initialize the clone
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     Property name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     PropertyInfo  to evaluate
        /// </summary>
        public PropertyInfo AssemblyPropertyInfo { get; set; }

        /// <summary>
        ///     Property Type
        /// </summary>
        [DataMember]
        public Type Type { get; set; }

        /// <summary>
        ///     Property Value
        /// </summary>
        [DataMember]
        public object Value { get; set; }
    }
}