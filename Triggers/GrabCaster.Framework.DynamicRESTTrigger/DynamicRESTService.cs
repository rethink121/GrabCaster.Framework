using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Globals;

namespace GrabCaster.Framework.DynamicRESTTrigger
{
    public class DynamicRESTService:IDynamicREST
    {
        private static Func<ActionTrigger, ActionContext> GetDataTrigger;

        static WebServiceHost engineHost;
        
        public string GetValue(string fromValue)
        {
            //execute the trigger event
            //witfor
            // RunTheMethod GetDataTrigger();
            return null;
        }

        public static bool StartService(string WebApiEndPoint,Func<ActionTrigger, ActionContext> getDataTrigger)
        {
            GetDataTrigger = getDataTrigger;

            engineHost = new WebServiceHost(typeof(DynamicRESTService), new Uri("http://localhost:8000"));
            engineHost.AddServiceEndpoint(typeof(DynamicRESTService), new WebHttpBinding(), ConfigurationBag.EngineName);
            var stp = engineHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = false;
            engineHost.Open();
            Thread.Sleep(Timeout.Infinite);
            return true;

        }
    }
}
