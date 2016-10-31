// AssemblyEvent.cs
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

namespace GrabCaster.Framework.Contracts.Bubbling
{
    /// <summary>
    ///     The assembly event.
    /// </summary>
    [DataContract]
    [Serializable]
    public class AssemblyEvent
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the path file name.
        /// </summary>
        [DataMember]
        public string PathFileName { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        ///     Gets or sets the assembly content.
        /// </summary>
        [DataMember]
        public byte[] AssemblyContent { get; set; }
    }
}