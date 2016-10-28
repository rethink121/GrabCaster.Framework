// SecuredWebExecuter.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
//   - Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//   - Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
//   
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

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
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => { return true; };

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
            catch (Exception)
            {
                // log and print out error
            }
            return null;
        }
    }
}