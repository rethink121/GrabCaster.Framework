using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.AssemblyFile;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Triggers;

namespace GrabCaster.Framework.Contracts.Components
{
    public interface IChainComponentAssembly:IAssemblyfile
    {
        //Trigger type activator
        IChainComponentType ChainComponentType { get; set; }

    }
}
