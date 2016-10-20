using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace HM.OMS.PageOneMessageRESTTrigger
{
    [ServiceContract]
    public interface IPageOneMessage
    {
        [OperationContract]
        [WebInvoke(Method= "POST",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                     UriTemplate = "json")]
        string SendMessage(iPageOneMessage pageOneMessage);

        [OperationContract]
        [WebGet]
        bool ServiceAvailable();

        [OperationContract]
        [WebGet]
        string Auth();

    }

    public interface iPageOneMessage
    {
        string From { get; set; }
        string To { get; set; }
        string Message { get; set; }

    }
}
