// --------------------------------------------------------------------------------------------------
// <copyright file = "BubblingEvent.cs" company="GrabCaster Ltd">
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

using GrabCaster.Framework.Contracts.Triggers;

namespace GrabCaster.Framework.Contracts.Bubbling
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// The bubbling event type.
    /// </summary>
    public enum BubblingEventType
    {
        /// <summary>
        /// The trigger.
        /// </summary>
        Trigger,
        /// <summary>
        /// The event.
        /// </summary>
        Event,

        /// <summary>
        /// The component.
        /// </summary>
        Component,
    }

    /// <summary>
    ///     Trigger Bubbling
    /// </summary>
    [DataContract]
    [Serializable]
    public class BubblingObject : IBubblingObject
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BubblingObject"/> class.
        /// </summary>
        public BubblingObject(byte[] data)
        {
            this.Events = new List<Event>();
            this.Properties = new Dictionary<string, Property>();
            this.Data = data;
        }




        /// <summary>
        /// High Availability Group .
        /// </summary>
        [DataMember]
        public string HAGroup { get; set; }
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        [DataMember]
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether no operation.
        /// </summary>
        [DataMember]
        public bool Nop { get; set; }

        /// <summary>
        /// Gets or sets the correlation.
        /// </summary>
        [DataMember]
        public Correlation Correlation { get; set; }

        /// <summary>
        /// Gets or sets the correlation override.
        /// </summary>
        [DataMember]
        public Correlation CorrelationOverride { get; set; }

        /// <summary>
        /// Gets or sets the bubbling event type.
        /// </summary>
        [DataMember]
        public BubblingEventType BubblingEventType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the id component.
        /// </summary>
        [DataMember]
        public string IdComponent { get; set; }

        /// <summary>
        /// Gets or sets the id configuration.
        /// </summary>
        [DataMember]
        public string IdConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the id chains.
        /// </summary>
        [DataMember]
        public List<Chain> Chains { get; set; }

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

        public Dictionary<string, Property> Properties { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        [DataMember]
        public Dictionary<string,Property> PropertiesContextProperties { get; set; }

        public IDictionary<string, string> PropertiesContext { get; set; }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<Event> Events { get; set; }

        /// <summary>
        /// Used to transport the raw data betwwen the source and the destination point
        /// Just the raw data picked up by the trigger or event
        /// </summary>
        [DataMember]
        public byte[] DataContext { get; set; }
        //To manage the Sync Async behaviour
        /// <summary>
        /// Used to notify that the request/response is syncronous
        /// </summary>
        [DataMember]
        public bool Syncronous { get; set; }
        /// <summary>
        /// Define if the event is a local and not remotely, in that case the engine does not use the message provider
        /// </summary>
        [DataMember]
        public bool LocalEvent { get; set; }
        
        /// <summary>
        /// Token used to identify the delegate to search and execute when back to the source
        /// </summary>
        [DataMember]
        public string SyncronousToken { get; set; }
        /// <summary>
        /// If it's a response syncronous from event
        /// </summary>
        [DataMember]
        public bool SyncronousFromEvent { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public byte[] Data { get; set; }

        [DataMember]
        public string SubscriberId {get; set;}
        [DataMember]
        public string ByteArray {get; set;}
        [DataMember]
        public string DestinationChannelId {get; set;}
        [DataMember]
        public string DestinationPointId {get; set;}
        [DataMember]
        public string SenderChannelId {get; set;}
        [DataMember]
        public string SenderChannelName {get; set;}
        [DataMember]
        public string SenderChannelDescription {get; set;}
        [DataMember]
        public string SenderPointId { get; set; }
        [DataMember]
        public string SenderName {get; set;}
        [DataMember]
        public string SenderDescriprion {get; set;}
        [DataMember]
        public string Embedded {get; set;}
        [DataMember]
        public string MessageType {get; set;}
        [DataMember]
        public string MessageId {get; set;}
        [DataMember]
        public bool Persisting {get; set;}
        [DataMember]
        public string Event {get; set;}
        [DataMember]
        public string Trigger {get; set;}
        [DataMember]
        public string SyncSendLocalDll {get; set;}
        [DataMember]
        public string SyncSendBubblingConfiguration {get; set;} //Send the bubbling configuration  {get; set;} the receiver will put in gcpoints
        [DataMember]
        public string SyncSendRequestBubblingConfiguration {get; set;}
        [DataMember]
        public string SyncSendFileBubblingConfiguration {get; set;}
        [DataMember]
        public string SyncSendRequestConfiguration {get; set;} //Send the request for all the configuration 
        [DataMember]
        public string SyncSendConfiguration {get; set;} //Send the request for all the configuration 
        [DataMember]
        public string SyncSendRequestComponent {get; set;} //Send a request to receive a component to sync
        [DataMember]
        public string TriggerEventJson {get; set;}
        [DataMember]
        public string EventPropertyJson {get; set;}
        [DataMember]
        public string SyncRequestConfirmed {get; set;}
        [DataMember]
        public string SyncAvailable {get; set;}
        [DataMember]
        public string ConsoleRequestSendBubblingBag {get; set;}
        [DataMember]
        public string ConsoleBubblingBagToSyncronize {get; set;}

        /// <summary>
        /// The serialize message.
        /// </summary>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] SerializeMessage(BubblingObject bubblingObject)
        {
            return GrabCaster.Framework.Serialization.Object.SerializationEngine.ObjectToByteArray(bubblingObject);
        }

        /// <summary>
        /// The serialize message.
        /// </summary>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static BubblingObject DeserializeMessage(byte[] byteArray)
        {
            return (BubblingObject)GrabCaster.Framework.Serialization.Object.SerializationEngine.ByteArrayToObject(byteArray);
        }
    }
}