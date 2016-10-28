// BubblingBag.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
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

using System;
using System.Collections.Generic;

namespace GrabCaster.Framework.Engine
{
    using Base;
    using Contracts.Bubbling;
    using Contracts.Configuration;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contains the bubbling folder filse (trg, evn, and dlls)
    /// </summary>
    [Serializable, DataContract]
    public class BubblingBagObjet
    {
        [DataMember]
        public List<TriggerConfiguration> TriggerConfigurationList { get; set; }

        [DataMember]
        public Dictionary<string, EventConfiguration> EventConfigurationList { get; set; }

        [DataMember]
        public List<ChainConfiguration> ChainConfigurationList { get; set; }

        [DataMember]
        public List<ComponentConfiguration> ComponentConfigurationList { get; set; }

        [DataMember]
        public List<BubblingObject> GlobalEventListBaseDll { get; set; }

        [DataMember]
        public Configuration Configuration { get; set; }
    }

    [Serializable]
    public class BubblingBag
    {
        public byte[] contentBubblingFolder { get; set; }
    }
}