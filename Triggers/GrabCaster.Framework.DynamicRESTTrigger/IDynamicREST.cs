using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.DynamicRESTTrigger
{
    [ServiceContract]
    public interface IDynamicREST
    {
        [OperationContract]
        [WebGet]
        string GetValue(string fromValue);
    }
}
