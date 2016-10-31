// TriggerAssembly.cs
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
using GrabCaster.Framework.Contracts.AssemblyFile;
using GrabCaster.Framework.Contracts.Bubbling;

#endregion

namespace GrabCaster.Framework.Contracts.Triggers
{
    public class TriggerAssembly : IAssemblyfile, ITriggerAssembly
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Shared { get; set; }
        public string PollingRequired { get; set; }
        public string Nop { get; set; }
        public Version Version { get; set; }
        public byte[] AssemblyContent { get; set; }
        public Assembly AssemblyObject { get; set; }
        public Type AssemblyClassType { get; set; }
        public string AssemblyFile { get; set; }
        public List<BaseAction> BaseActions { get; set; }
        public Dictionary<string, Property> Properties { get; set; }
        public ITriggerType TriggerType { get; set; }
    }
}