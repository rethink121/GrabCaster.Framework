// PageOneRestApi.cs
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
using HMVN.OMS.Common;
using HMVN.OMS.Common.Utils;
using Nito.AspNetBackgroundTasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HM.OMS.PageOneMessageConsole
{
    public class PageOneRestApi
    {
        //readonly WebClientExecuter Executer;
        readonly string API = "https://www.oventus.com/rest/v1/";
        readonly string Username = "HMOMSSMS";
        readonly string Password = "Fw6Hf3Ne6";

        public PageOneRestApi()
        {
            //var headers = new WebHeaderCollection();
            //headers.Add(HttpRequestHeader.Accept, "application/json");
            //Executer = new WebClientExecuter(HttpHelper.UrlPathCombine(API, Username), headers, Method.GET);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>{"accountID":"[unique_account_id]","username":"[username]","status":{"@description":"User Authenticated","$":"200"}}</returns>
        public string Authentication()
        {
            return ExecuteAPI(ApiObject.Authentication);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>{"@transactionID":"14024254","status":{"@description":"Accepted","$":"201"}}</returns>
        public string SendMessage(SendMessageRequest data)
        {
            return ExecuteAPI(ApiObject.SendMessage, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Number of Credit</returns>
        public string Credit()
        {
            return ExecuteAPI(ApiObject.Credit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>{"Msisdn":[{"msisdn":"44700000001","keyword":"*"},{"msisdn":"AlphaTag","keyword":"*"}]}</returns>
        public string MSISDN()
        {
            return ExecuteAPI(ApiObject.MSISDN);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">radius, nMinutes, lat, longitude, maxResuls</param>
        /// <returns>location object</returns>
        public string FindWithin(Dictionary<string, object> data)
        {
            return ExecuteAPI(ApiObject.FindWithin, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>location object</returns>
        public string Locate(string deviceAddress)
        {
            return ExecuteAPI(ApiObject.Locate, new { address = deviceAddress });
        }

        string ExecuteAPI(ApiObject apiObject, object data = null)
        {
            string result = string.Empty;
            string url = HttpHelper.UrlPathCombine(API, Username);
            if (string.IsNullOrWhiteSpace(apiObject.Path) == false)
                url = HttpHelper.UrlPathCombine(url, apiObject.Path);
            url += string.Format("?password={0}", Password);

            if (apiObject.AsyncNoResponse)
                BackgroundTaskManager.Run(() => Execute(apiObject.Method, url, data, null, 100));
            else
                result = Execute(apiObject.Method, url, data, null, 100);

            return result;
        }

        public string Execute(Method method, string apiPath, object data, ErrorHandler handler, int timeOutInSecond)
        {
            string response = string.Empty;
            try
            {
                using (WebClient client = new WebClient())
                {
                    string url = apiPath;
                    client.Encoding = System.Text.Encoding.UTF8;
                    var values = PrepareExecute(client, ref url, data, method);
                    if (method == Method.GET)
                        response = client.DownloadString(url);
                    else
                    {
                        response = Encoding.UTF8.GetString(client.UploadValues(url, method.ToString(), values));
                    }
                }
            }
            catch (WebException wexc)
            {
                string error = HttpHelper.CatchWebRequestException(wexc);
                if (handler != null)
                {
                    HttpStatusCode code = (wexc.Response as HttpWebResponse).StatusCode;
                    if (code == HttpStatusCode.BadRequest && handler.E400 != null)
                        handler.E400.Invoke(error);
                    else if (code == HttpStatusCode.Unauthorized && handler.E401 != null)
                        handler.E401.Invoke(error);
                    else if (code == HttpStatusCode.InternalServerError && handler.E500 != null)
                        handler.E500.Invoke(error);
                    else if (handler.AnyError != null)
                        handler.AnyError.Invoke(error);
                }
                HttpHelper.WriteLogError(error);
                response = error;
            }
            catch (Exception ex)
            {
                HttpHelper.WriteLogError(ex);
                response = ex.Message;
            }


            return response;
        }

        NameValueCollection PrepareExecute(WebClient client, ref string url, object data, Method method = Method.GET)
        {
            //string jsonRequest = string.Empty;
            client.Headers = new WebHeaderCollection();
            client.Headers[HttpRequestHeader.Accept] = "application/json; charset=utf-8";
            if (data == null)
                return null;

            var values = new NameValueCollection();
            foreach (var p in data.GetType().GetProperties())
            {
                var value = p.GetValue(data);
                if (p.PropertyType.IsArray || p.PropertyType.IsGenericType)
                {
                    int counter = 0;
                    foreach (var subItem in (IEnumerable)value)
                    {
                        values[p.Name.ToLower()] = subItem.ToString();
                        counter++;
                    }
                }
                else
                    values[p.Name.ToLower()] = value.ToString();
            }

            if (method == Method.GET)
            {
                string paramString = HttpHelper.DictToString(values);
                url = HttpHelper.UrlParamCombine(url, paramString);
            }
            else
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            }

            HttpHelper.WriteLogMessage(string.Format("Url: {0} - data: {1}", url, HttpHelper.DictToString(values)));
            return values;
        }

        internal class ApiObject
        {
            public static readonly ApiObject Authentication = new ApiObject { Path = "", Method = Method.GET };
            public static readonly ApiObject SendMessage = new ApiObject { Path = "message", Method = Method.POST };
            public static readonly ApiObject Credit = new ApiObject { Path = "credits", Method = Method.GET };
            public static readonly ApiObject MSISDN = new ApiObject { Path = "msisdn", Method = Method.GET };
            public static readonly ApiObject FindWithin = new ApiObject { Path = "findwithin", Method = Method.GET };
            public static readonly ApiObject Locate = new ApiObject { Path = "locate", Method = Method.GET };

            internal string Path { get; set; }
            internal Method Method { get; set; }
            internal bool AsyncNoResponse { get; set; }
        }

        public class SendMessageRequest
        {
            public List<string> To { get; set; }
            public string Message { get; set; }
            public string From { get; set; }
        }
    }

    public enum Method
    {
        NONE,
        POST,
        GET,
        PUT,
        DELETE
    }

    public class ErrorHandler
    {
        public Action<string> E400 { get; set; }
        public Action<string> E500 { get; set; }
        public Action<string> E401 { get; set; }
        public Action<string> AnyError { get; set; }
    }
}
