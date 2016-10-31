// ComponentConfiguration.cs
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

#endregion

namespace GrabCaster.Framework.Contracts.Configuration
{
    /// <summary>
    ///     Component event File
    /// </summary>
    [DataContract]
    [Serializable]
    public class ComponentConfiguration
    {
        /// <summary>
        ///     Gets or sets the event.
        /// </summary>
        [DataMember]
        public Component Component { get; set; }
    }

    /// <summary>
    ///     The component.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Component
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="idComponent">
        ///     The id component.
        /// </param>
        /// <param name="idConfiguration">
        ///     The id configuration.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="description">
        ///     The description.
        /// </param>
        public Component(string idComponent, string name, string description)
        {
            IdComponent = idComponent;
            Name = name;
            Description = description;
        }

        /// <summary>
        ///     Gets or sets the id component.
        /// </summary>
        [DataMember]
        public string IdComponent { get; set; }

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
        ///     Gets or sets the event properties.
        /// </summary>
        [DataMember]
        public List<ComponentProperty> ComponentProperties { get; set; }
    }

    /// <summary>
    ///     The event property.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ComponentProperty
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventProperty" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public ComponentProperty(string name, object value)
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
}