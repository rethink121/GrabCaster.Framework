using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HM.OMS.PageOneMessageConsole.Utilities
{
    public class SecuredWebExecuter
    {
        public string Execute(string url)
        {
            try
            {
                // use the SSL protocol (instead of TLS)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                // ignore any certificate complaints
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

                // create HTTP web request with proper content type
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/xml;charset=UTF8";

                // grab the PFX as a X.509 certificate from disk
                string certFileName = Path.Combine("webservice.pfx");

                // load the X.509 certificate and add to the web request
                X509Certificate cert = new X509Certificate(certFileName, "(top-secret password)");
                request.ClientCertificates.Add(cert);
                request.PreAuthenticate = true;

                // call the web service and get response
                WebResponse response = request.GetResponse();

                Stream responseStream = response.GetResponseStream();
            }
            catch (Exception exc)
            {
                // log and print out error
            }
            return null;
        }
    }
}
