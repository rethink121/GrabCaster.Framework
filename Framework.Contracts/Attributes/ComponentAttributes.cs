// ComponentAttributes.cs
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
using System.Runtime.Serialization;

#endregion

namespace GrabCaster.Framework.Contracts.Attributes
{
    /// <summary>
    ///     The component contract.
    /// </summary>
    [DataContract]
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)] // Multiuse attribute.
    public class ComponentContract : Attribute
    {
        public ComponentContract(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

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
    }

    /// <summary>
    ///     The component property property contract.
    /// </summary>
    [DataContract]
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class ComponentPropertyContract : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TriggerPropertyContract" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="description">
        ///     The description.
        /// </param>
        public ComponentPropertyContract(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        ///     Property name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }

    /// <summary>
    ///     The event action contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ComponentActionContract : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventActionContract" /> class.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="description">
        ///     The description.
        /// </param>
        public ComponentActionContract(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        /// <summary>
        ///     Unique Action ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}