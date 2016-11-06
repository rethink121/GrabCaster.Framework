// IBubblingObject.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using System.Collections.Generic;
using GrabCaster.Framework.Contracts.Configuration;

#endregion

namespace GrabCaster.Framework.Contracts.Bubbling
{
    /// <summary>
    ///     The BubblingEvent interface.
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
        ///     Gets or sets the id configuration.
        /// </summary>
        string IdConfiguration { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether shared.
        /// </summary>
        bool Shared { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether polling required.
        /// </summary>
        bool PollingRequired { get; set; }

        /// <summary>
        ///     Gets or sets the properties.
        /// </summary>
        Dictionary<string, Property> Properties { get; set; }

        /// <summary>
        ///     Gets or sets the properties.
        /// </summary>
        IDictionary<string, string> PropertiesContext { get; set; }


        /// <summary>
        ///     Gets or sets the events.
        /// </summary>
        List<Event> Events { get; set; }

        /// <summary>
        ///     Used to transport the raw data betwwen the source and the destination point
        ///     Just the raw data picked up by the trigger or event
        /// </summary>
        byte[] DataContext { get; set; }

        //To manage the Sync Async behaviour
        /// <summary>
        ///     Used to notify that the request/response is syncronous
        /// </summary>
        bool Syncronous { get; set; }

        /// <summary>
        ///     Token used to identify the delegate to search and execute when back to the source
        /// </summary>
        string SyncronousToken { get; set; }

        /// <summary>
        ///     If it's a response syncronous from event
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

        string SyncSendBubblingConfiguration { get; set; }
        //Send the bubbling configuration  {get; set;} the receiver will put in gcpoints

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