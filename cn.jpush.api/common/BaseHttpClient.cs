using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using cn.jpush.api.common.resp;

namespace cn.jpush.api.common
{
    internal class BaseHttpClient
    {
        private const string CHARSET = "UTF-8";

        private const string RATE_LIMIT_QUOTA = "X-Rate-Limit-Limit";

        private const string RATE_LIMIT_Remaining = "X-Rate-Limit-Remaining";

        private const string RATE_LIMIT_Reset = "X-Rate-Limit-Reset";

        protected const int RESPONSE_OK = 200;

        //设置连接超时时间
        private const int DEFAULT_CONNECTION_TIMEOUT = 20 * 1000; // milliseconds

        //设置读取超时时间
        private const int DEFAULT_SOCKET_TIMEOUT = 30 * 1000; // milliseconds

        public ResponseWrapper sendPost(string url, string auth, string reqParams)
        {
            return sendRequest("POST", url, auth, reqParams);
        }

        public ResponseWrapper sendDelete(string url, string auth, string reqParams)
        {
            return sendRequest("DELETE", url, auth, reqParams);
        }

        public ResponseWrapper sendGet(string url, string auth, string reqParams)
        {
            return sendRequest("GET", url, auth, reqParams);
        }

        /**
         *
         * method "POST" or "GET"
         * url
         * auth   可选
         */

        public ResponseWrapper sendRequest(string method, string url, string auth, string reqParams)
        {
            Debug.WriteLine("Send request - " + method + " " + url + " " + DateTime.Now);
            if (null != reqParams)
            {
                Debug.WriteLine("Request Content - " + reqParams + " " + DateTime.Now);
            }
            var result = new ResponseWrapper();
            HttpWebRequest myReq = null;
            HttpWebResponse response = null;
            try
            {
                myReq = (HttpWebRequest)WebRequest.Create(url);
                myReq.Method = method;
                myReq.ContentType = "application/json";
                if (!string.IsNullOrEmpty(auth))
                {
                    myReq.Headers.Add("Authorization", "Basic " + auth);
                }
                if (method == "POST")
                {
                    var bs = Encoding.UTF8.GetBytes(reqParams);
                    myReq.ContentLength = bs.Length;
                    using (var reqStream = myReq.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                        reqStream.Close();
                    }
                }
                response = (HttpWebResponse)myReq.GetResponse();
                var statusCode = response.StatusCode;
                result.responseCode = statusCode;
                if (Equals(response.StatusCode, HttpStatusCode.OK))
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result.responseContent = reader.ReadToEnd();
                    }
                    var limitQuota = response.GetResponseHeader(RATE_LIMIT_QUOTA);
                    var limitRemaining = response.GetResponseHeader(RATE_LIMIT_Remaining);
                    var limitReset = response.GetResponseHeader(RATE_LIMIT_Reset);
                    result.setRateLimit(limitQuota, limitRemaining, limitReset);
                    Debug.WriteLine("Succeed to get response - 200 OK" + " " + DateTime.Now);
                    Debug.WriteLine("Response Content - {0}", result.responseContent + " " + DateTime.Now);
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var errorCode = ((HttpWebResponse)e.Response).StatusCode;
                    var statusDescription = ((HttpWebResponse)e.Response).StatusDescription;
                    using (var sr = new StreamReader(((HttpWebResponse)e.Response).GetResponseStream(), Encoding.UTF8))
                    {
                        result.responseContent = sr.ReadToEnd();
                    }
                    result.responseCode = errorCode;
                    result.exceptionString = e.Message;
                    var limitQuota = ((HttpWebResponse)e.Response).GetResponseHeader(RATE_LIMIT_QUOTA);
                    var limitRemaining = ((HttpWebResponse)e.Response).GetResponseHeader(RATE_LIMIT_Remaining);
                    var limitReset = ((HttpWebResponse)e.Response).GetResponseHeader(RATE_LIMIT_Reset);
                    result.setRateLimit(limitQuota, limitRemaining, limitReset);
                    Debug.Print(e.Message);
                    result.setErrorObject();
                    Debug.WriteLine(string.Format("fail  to get response - {0}", errorCode) + " " + DateTime.Now);
                    Debug.WriteLine(string.Format("Response Content - {0}", result.responseContent) + " " + DateTime.Now);

                    throw new APIRequestException(result);
                }
                else
                {
//
                    throw new APIConnectionException(e.Message);
                }
            }
                //这里不再抓取非http的异常，如果异常抛出交给开发者自行处理
                //catch (System.Exception ex)
                //{
                //     String errorMsg = ex.Message;
                //     Debug.Print(errorMsg);
                //}
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (myReq != null)
                {
                    myReq.Abort();
                }
            }
            return result;
        }
    }
}