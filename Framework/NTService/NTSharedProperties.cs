// --------------------------------------------------------------------------------------------------
// <copyright file = "NTSharedProperties.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.NTService
{
    using System.ServiceProcess;

    /// <summary>
    /// Properties for the Windows Service.
    /// </summary>
    internal static class NtSharedProperties
    {
        /// <summary>
        /// Gets or sets the account for the Windows Service.
        /// </summary>
        /// <value>
        /// The account for the Windows Service.
        /// </value>
        public static ServiceAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the password for the Windows Service.
        /// </summary>
        /// <value>
        /// The password for the Windows Service.
        /// </value>
        public static string Password { get; set; }

        /// <summary>
        /// Gets or sets the username for the Windows Service.
        /// </summary>
        /// <value>
        /// The username for the Windows Service.
        /// </value>
        public static string Username { get; set; }

        /// <summary>
        /// Gets or sets the name for the Windows Service.
        /// </summary>
        /// <value>
        /// The name for the Windows Service.
        /// </value>
        public static string WindowsNtServiceName { get; set; }

        /// <summary>
        /// Gets or sets the display name for the Windows Service.
        /// </summary>
        /// <value>
        /// The display name for the Windows Service.
        /// </value>
        public static string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description for the Windows Service.
        /// </summary>
        /// <value>
        /// The description for the Windows Service.
        /// </value>
        public static string Description { get; set; }
    } // NTSharedProperties
} // namespace