using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.AssemblyFile;

namespace GrabCaster.Framework.Contracts.Events
{
    public interface IEventAssembly:IAssemblyfile
    {
        IEventType EventType { get; set; }
    }
}
