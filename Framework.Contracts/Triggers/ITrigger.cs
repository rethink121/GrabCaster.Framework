// --------------------------------------------------------------------------------------------------
// <copyright file = "ITrigger.cs" company="GrabCaster Ltd">
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

using System;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using GrabCaster.Framework.Contracts.Attributes;

namespace GrabCaster.Framework.Contracts.Triggers
{
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The TriggerType interface.
    /// </summary>
    public interface ITriggerType
    {
        /// <summary>
        /// Internal Trigger context.
        /// </summary>
        ActionContext Context { get; set; }

        /// <summary>
        /// internal delegate to use in delegates events
        /// </summary>
        ActionTrigger ActionTrigger { get; set; }

        /// <summary>
        /// Identify if the trigger request must be syncronous or not
        /// </summary>
        bool Syncronous { get; set; }

        /// <summary>
        ///     Main default data property
        /// </summary>
        byte[] DataContext { get; set; }

        /// <summary>
        /// Main default method
        /// </summary>
        /// <param name="actionTrigger">
        /// The set Event Action Trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        byte[] Execute(ActionTrigger actionTrigger, ActionContext context);
    }
}