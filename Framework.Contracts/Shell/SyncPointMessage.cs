// SyncPointMessage.cs
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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using GrabCaster.Framework.Contracts.Points;

#endregion

namespace GrabCaster.Framework.Contracts.Shell
{
    /// <summary>
    ///     The shell item type.
    /// </summary>
    public enum ShellItemType
    {
        /// <summary>
        ///     The host.
        /// </summary>
        Host,

        /// <summary>
        ///     The point.
        /// </summary>
        Point,

        /// <summary>
        ///     The trigger dll.
        /// </summary>
        TriggerDll,

        /// <summary>
        ///     The event dll.
        /// </summary>
        EventDll,

        /// <summary>
        ///     The trigger configuration.
        /// </summary>
        TriggerConfiguration,

        /// <summary>
        ///     The event configuration.
        /// </summary>
        EventConfiguration
    }

    /// <summary>
    ///     The sync point message.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SyncPointMessage
    {
        /// <summary>
        ///     Gets or sets the host.
        /// </summary>
        [DataMember]
        public Host Host { get; set; }

        /// <summary>
        ///     Gets or sets the point.
        /// </summary>
        [DataMember]
        public Point Point { get; set; }

        /// <summary>
        ///     Gets or sets the bubbling triggers.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> BubblingTriggers { get; set; }

        /// <summary>
        ///     Gets or sets the bubbling events.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> BubblingEvents { get; set; }

        /// <summary>
        ///     Gets or sets the triggers.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> DllTriggers { get; set; }

        /// <summary>
        ///     Gets or sets the events.
        /// </summary>
        [DataMember]
        public List<SyncPointFile> DllEvents { get; set; }
    }

    /// <summary>
    ///     The host.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Host
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the ip.
        /// </summary>
        [DataMember]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
             Justification = "Reviewed. Suppression is OK here.")]
        public string Ip { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}