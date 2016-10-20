using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;

namespace GrabCaster.Framework.Contracts
{
    public class TriggerEmbeddedBag
    {
        public ActionTrigger DelegateActionTrigger { get; set; }
        public ActionContext ActionContextTrigger { get; set; }

        public BaseAction BaseActionTrigger { get; set; }
        public object[] Parameters { get; set; }
        public ITriggerType ITriggerTypeInstance { get; set; }
        public IEventType IEventTypeInstance { get; set; }
        public ActionContext ActionContext { get; set; }
     
        public List<Property> Properties { get; set; }
    }
}
