// --------------------------------------------------------------------------------------------------
// <copyright file = "IAction.cs" company="GrabCaster Ltd">
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
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    internal interface IAction
    {
        /// <summary>
        ///     Unique Action ID
        /// </summary>   
        string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Internal Method to invoke
        /// </summary>
        MethodInfo AssemblyMethodInfo { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///     Property Value
        /// </summary>
        object ReturnValue { get; set; }

        /// <summary>
        ///     Bubbling parameters
        /// </summary>
        List<Parameter> Parameters { get; set; }
    }
}