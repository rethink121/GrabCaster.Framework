// LogMessage.cs
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
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.Storage.Table;

#endregion

namespace GrabCaster.Framework.Contracts.Log
{
    /// <summary>
    ///     The message level.
    /// </summary>
    public enum MessageLevel
    {
        /// <summary>
        ///     The information.
        /// </summary>
        Information,

        /// <summary>
        ///     The warning.
        /// </summary>
        Warning,

        /// <summary>
        ///     The error.
        /// </summary>
        Error
    }

    /// <summary>
    ///     The log message.
    /// </summary>
    [DataContract]
    [Serializable]
    public class LogMessage : TableEntity
    {
        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        [DataMember]
        public int Level { get; set; }

        /// <summary>
        ///     Gets or sets the message id.
        /// </summary>
        [DataMember]
        public string MessageId { get; set; }

        /// <summary>
        ///     Gets or sets the date time.
        /// </summary>
        [DataMember]
        public string DateTime { get; set; }

        /// <summary>
        ///     Gets or sets the source.
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        ///     Gets or sets the channel id.
        /// </summary>
        [DataMember]
        public string PointId { get; set; }

        /// <summary>
        ///     Gets or sets the channel name.
        /// </summary>
        [DataMember]
        public string PointName { get; set; }

        /// <summary>
        ///     Gets or sets the channel id.
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        ///     Gets or sets the channel name.
        /// </summary>
        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        ///     Gets or sets the event id.
        /// </summary>
        [DataMember]
        public int EventId { get; set; }

        /// <summary>
        ///     Gets or sets the task category.
        /// </summary>
        [DataMember]
        public string TaskCategory { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the exception object.
        /// </summary>
        [DataMember]
        public string ExceptionObject { get; set; }
    }
}