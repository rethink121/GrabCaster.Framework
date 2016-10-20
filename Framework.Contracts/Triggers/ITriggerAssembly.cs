using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.AssemblyFile;

namespace GrabCaster.Framework.Contracts.Triggers
{
    public interface ITriggerAssembly:IAssemblyfile
    {
        ITriggerType TriggerType { get; set; }
    }
}
