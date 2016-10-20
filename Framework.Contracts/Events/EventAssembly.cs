﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.AssemblyFile;
using GrabCaster.Framework.Contracts.Bubbling;

namespace GrabCaster.Framework.Contracts.Events
{
    public class EventAssembly:IEventAssembly
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Shared { get; set; }
        public string PollingRequired { get; set; }
        public string Nop { get; set; }
        public Version Version { get; set; }
        public byte[] AssemblyContent { get; set; }
        public IEventType EventType { get; set; }
        public Assembly AssemblyObject { get; set; }
        public Type AssemblyClassType { get; set; }
        public string AssemblyFile { get; set; }
        public List<BaseAction> BaseActions { get; set; }
        public Dictionary<string, Property> Properties { get; set; }
    }
}
