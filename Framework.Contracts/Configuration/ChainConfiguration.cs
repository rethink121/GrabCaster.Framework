// ChainConfiguration.cs
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
    public class ChainConfiguration
    {
        /// <summary>
        ///     Gets or sets the event.
        /// </summary>
        [DataMember]
        public Chain Chain { get; set; }
    }

    /// <summary>
    ///     The component.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Chain
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="idChain">
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
        public Chain(string idChain, string name, string description)
        {
            IdChain = idChain;
            Name = name;
            Description = description;
        }

        /// <summary>
        ///     Gets or sets the id chain.
        /// </summary>
        [DataMember]
        public string IdChain { get; set; }

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
        public List<ComponentBag> Components { get; set; }
    }

    /// <summary>
    ///     The event property.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ComponentBag
    {
        public ComponentBag(string idcomponent)
        {
            idComponent = idcomponent;
        }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        [DataMember]
        public string idComponent { get; set; }
    }
}