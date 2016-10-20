// --------------------------------------------------------------------------------------------------
// <copyright file = "BubblingBag.cs" company="GrabCaster Ltd">
//   Copyright (c) 2013 - 2016 GrabCaster Ltd All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
// 
//    Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Engine
{
    using System.Runtime.Serialization;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Configuration;

    /// <summary>
    /// Contains the bubbling folder filse (trg, evn, and dlls)
    /// </summary>
    [Serializable,DataContract]
    public class BubblingBagObjet
    {
        [DataMember]
        public List<TriggerConfiguration> TriggerConfigurationList { get; set; }
        [DataMember]
        public Dictionary<string,EventConfiguration> EventConfigurationList { get; set; }

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
