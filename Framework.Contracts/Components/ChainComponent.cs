using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.Bubbling;

namespace GrabCaster.Framework.Contracts.Components
{
    class ChainComponent
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        [DataMember]
        public List<Property> Properties { get; set; }
    }
}
