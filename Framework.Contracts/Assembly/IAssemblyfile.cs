using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Events;

namespace GrabCaster.Framework.Contracts.AssemblyFile
{
    public interface IAssemblyfile
    {

       
        string Id { get; set; }

        string Name { get; set; }
        string Description { get; set; }
        string Shared { get; set; }
        string PollingRequired { get; set; }
        string Nop { get; set; }
        System.Version Version { get; set; }
        /// <summary>
        /// Gets or sets the assembly content.
        /// </summary>
        byte[] AssemblyContent { get; set; }
        /// <summary>
        ///     Assembly object ready to invoke (performances)
        /// </summary>
        System.Reflection.Assembly AssemblyObject { get; set; }

        /// <summary>
        ///     Internal class type to invoke
        /// </summary>
        Type AssemblyClassType { get; set; }
        /// <summary>
        /// Gets or sets the assembly file.
        /// </summary>
        string AssemblyFile { get; set; }

        /// <summary>
        /// Gets or sets the base actions.
        /// </summary>
        List<BaseAction> BaseActions { get; set; }

        Dictionary<string,Property> Properties { get; set; }
    }
}
