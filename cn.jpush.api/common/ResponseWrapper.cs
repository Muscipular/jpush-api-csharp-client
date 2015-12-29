using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using cn.jpush.api.push;
using cn.jpush.api.util;
using Newtonsoft.Json;

namespace cn.jpush.api.common
{
    public class ResponseWrapper
    {
        private const int RESPONSE_CODE_NONE = -1;

        public string exceptionString;

        //private static Gson _gson = new Gson();
        public JpushError jpushError;

        public int rateLimitQuota;

        public int rateLimitRemaining;

        public int rateLimitReset;

        public HttpStatusCode responseCode = HttpStatusCode.BadRequest;

        public string responseContent { get; set; }

        public void setErrorObject()
        {
            if (!string.IsNullOrEmpty(responseContent))
            {
                jpushError = JsonConvert.DeserializeObject<JpushError>(responseContent);
            }
        }

        public bool isServerResponse()
        {
            return responseCode == HttpStatusCode.OK;
        }

        public void setRateLimit(string quota, string remaining, string reset)
        {
            if (null == quota)
            {
                return;
            }
            try
            {
                int value;
                if (!string.IsNullOrEmpty(quota) && int.TryParse(quota, out value))
                {
                    rateLimitQuota = value;
                }
                if (!string.IsNullOrEmpty(remaining) && int.TryParse(remaining, out value))
                {
                    rateLimitRemaining = value;
                }
                if (!string.IsNullOrEmpty(reset) && int.TryParse(reset, out value))
                {
                    rateLimitReset = value;
                }
                Debug.WriteLine(string.Format("JPush API Rate Limiting params - quota:{0}, remaining:{1}, reset:{2} ", quota, remaining, reset) + " " + DateTime.Now);
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
        }
    }
}