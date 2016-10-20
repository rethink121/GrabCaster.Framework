// --------------------------------------------------------------------------------------------------
// <copyright file = "IBubblingEvent.cs" company="GrabCaster Ltd">
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
//    Reciprocal License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Contracts.Bubbling
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// The BubblingEvent interface.
    /// </summary>
    public interface IBubblingObject
    {


        byte[] Data { get; set; }

        /// <summary>
        ///     Trigger or Event
        /// </summary>
        BubblingEventType BubblingEventType { get; set; }

        /// <summary>
        ///     If trigger type and a file trigger event is present in the Bubbling directory this prop is true
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        ///     Endpoints destinations
        /// </summary>
        string IdComponent { get; set; }

        /// <summary>
        /// Gets or sets the id configuration.
        /// </summary>
        string IdConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared.
        /// </summary>
        bool Shared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether polling required.
        /// </summary>
        bool PollingRequired { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        Dictionary<string, Property> Properties { get; set; }
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        IDictionary<string, string> PropertiesContext { get; set; }


        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        List<Event> Events { get; set; }
        /// <summary>
        /// Used to transport the raw data betwwen the source and the destination point
        /// Just the raw data picked up by the trigger or event
        /// </summary>
        byte[] DataContext { get; set; }
        //To manage the Sync Async behaviour
        /// <summary>
        /// Used to notify that the request/response is syncronous
        /// </summary>
        bool Syncronous { get; set; }
        /// <summary>
        /// Token used to identify the delegate to search and execute when back to the source
        /// </summary>
        string SyncronousToken { get; set; }
        /// <summary>
        /// If it's a response syncronous from event
        /// </summary>
        bool SyncronousFromEvent { get; set; }
        
        string SubscriberId { get; set; }
        
        string ByteArray { get; set; }
        
        string DestinationChannelId { get; set; }
        
        string DestinationPointId { get; set; }
        
        string SenderChannelId { get; set; }
        
        string SenderChannelName { get; set; }
        
        string SenderChannelDescription { get; set; }
        
        string SenderPointId { get; set; }
        
        string SenderName { get; set; }
        
        string SenderDescriprion { get; set; }
        
        string Embedded { get; set; }
        
        string MessageType { get; set; }
        
        string MessageId { get; set; }
        
        bool Persisting { get; set; }
        
        string Event { get; set; }
        
        string Trigger { get; set; }
        
        string SyncSendLocalDll { get; set; }
        
        string SyncSendBubblingConfiguration { get; set; } //Send the bubbling configuration  {get; set;} the receiver will put in gcpoints
        
        string SyncSendRequestBubblingConfiguration { get; set; }
        
        string SyncSendFileBubblingConfiguration { get; set; }
        
        string SyncSendRequestConfiguration { get; set; } //Send the request for all the configuration 
        
        string SyncSendConfiguration { get; set; } //Send the request for all the configuration 
        
        string SyncSendRequestComponent { get; set; } //Send a request to receive a component to sync
        
        string TriggerEventJson { get; set; }
        
        string EventPropertyJson { get; set; }
        
        string SyncRequestConfirmed { get; set; }
        
        string SyncAvailable { get; set; }
        
        string ConsoleRequestSendBubblingBag { get; set; }
        
        string ConsoleBubblingBagToSyncronize { get; set; }
    }
}