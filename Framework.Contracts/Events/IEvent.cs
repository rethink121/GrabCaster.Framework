// --------------------------------------------------------------------------------------------------
// <copyright file = "IEvent.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Contracts.Events
{
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// Interface for all event action classes.
    /// </summary>
    public interface IEventType
    {
        /// <summary>
        /// Gets or sets the internal context passed to the event (some other event to execute) to use in delegates events.
        /// </summary>
        /// <returns>The internal context passed to the event.</returns>
        ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the internal delegate to use in delegates events.
        /// </summary>
        /// <value>
        /// The set event action event.
        /// </value>
        ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the main default data.
        /// </summary>
        /// <value>
        /// The main default data.
        /// </value>
        byte[] DataContext { get; set; }

        /// <summary>
        /// Performs the execution of the event.
        /// </summary>
        /// <param name="actionEvent">The The internal delegate to use</param>
        /// <param name="context">The internal context passed to the event.</param>
        byte[] Execute(ActionEvent actionEvent, ActionContext context);
    }
}