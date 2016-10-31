// BaseAction.cs
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
using System.Reflection;
using System.Runtime.Serialization;

#endregion

namespace GrabCaster.Framework.Contracts.Bubbling
{
    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    [DataContract]
    [Serializable]
    public class BaseAction : IAction
    {
        public BaseAction(string ID, string Name, string Description, MethodInfo AssemblyMethodInfo, string ReturnValue)
        {
            Id = ID;
            this.Name = Name;
            this.Description = Description;
            this.AssemblyMethodInfo = AssemblyMethodInfo;
            this.ReturnValue = ReturnValue;
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

        /// <summary>
        ///     Internal Method to invoke
        /// </summary>
        public MethodInfo AssemblyMethodInfo { get; set; }

        /// <summary>
        ///     Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Property Value
        /// </summary>
        [DataMember]
        public object ReturnValue { get; set; }

        /// <summary>
        ///     Bubbling parameters
        /// </summary>
        [DataMember]
        public List<Parameter> Parameters { get; set; }
    }
}