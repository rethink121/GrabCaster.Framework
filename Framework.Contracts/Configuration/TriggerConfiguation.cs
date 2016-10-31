// TriggerConfiguation.cs
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
//   - Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//   - Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
//   
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

#region Usings

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GrabCaster.Framework.Contracts.Channels;

#endregion

namespace GrabCaster.Framework.Contracts.Configuration
{
    /// <summary>
    ///     Trigger configuration File, To create a configuration file trigger who able to activate a trigger action/s
    /// </summary>
    [DataContract]
    [Serializable]
    public class TriggerConfiguration
    {
        /// <summary>
        ///     Gets or sets the trigger.
        /// </summary>
        [DataMember]
        public Trigger Trigger { get; set; }

        /// <summary>
        ///     Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<Event> Events { get; set; }
    }

    /// <summary>
    ///     The trigger.
    /// </summary>
    [DataContract, Serializable]
    public class Trigger
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Trigger" /> class.
        /// </summary>
        /// <param name="idComponent">
        ///     The id component.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="description">
        ///     The description.
        /// </param>
        public Trigger(string idComponent, string idConfiguration, string name, string description)
        {
            IdComponent = idComponent;
            IdConfiguration = idConfiguration;
            Name = name;
            Description = description;
        }


        /// <summary>
        ///     Gets or sets the id component.
        /// </summary>
        [DataMember]
        public string IdComponent { get; set; }

        /// <summary>
        ///     Get or set the id configuration
        /// </summary>
        [DataMember]
        public string IdConfiguration { get; set; }


        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the channels where the trigger could be executes, if null then can be executed anyway.
        /// </summary>
        [DataMember]
        public List<Channel> Channels { get; set; }

        /// <summary>
        ///     Gets or sets the id chains.
        /// </summary>
        [DataMember]
        public List<Chain> Chains { get; set; }

        /// <summary>
        ///     Gets or sets the trigger properties.
        /// </summary>
        [DataMember]
        public List<TriggerProperty> TriggerProperties { get; set; }
    }

    /// <summary>
    ///     The trigger property.
    /// </summary>
    [DataContract, Serializable]
    public class TriggerProperty
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TriggerProperty" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public TriggerProperty(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     Property name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        [DataMember]
        public object Value { get; set; }
    }

    /// <summary>
    ///     The trigger action.
    /// </summary>
    [DataContract, Serializable]
    public class TriggerAction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TriggerAction" /> class.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        public TriggerAction(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}