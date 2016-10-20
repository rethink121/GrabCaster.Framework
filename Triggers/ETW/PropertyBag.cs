// --------------------------------------------------------------------------------------------------
// <copyright file = "PropertyBag.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.ETW
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The property bag.
    /// </summary>
    [Serializable]
    public sealed class PropertyBag : Dictionary<string, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        public PropertyBag()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        public PropertyBag(int capacity)
            : base(capacity, StringComparer.Ordinal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        private PropertyBag(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}