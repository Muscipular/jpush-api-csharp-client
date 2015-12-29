using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using cn.jpush.api.common;

namespace cn.jpush.api.report
{
    public class ReceivedResult : BaseResult
    {
        public List<Received> ReceivedList { get; set; } = new List<Received>();

        public override bool isResultOK()
        {
            return ResponseResult.responseCode == HttpStatusCode.OK;
        }

        public HttpStatusCode getErrorCode()
        {
            if (null != ResponseResult)
            {
                return ResponseResult.responseCode;
            }
            return 0;
        }

        public string getErrorMessage()
        {
            if (null != ResponseResult)
            {
                return ResponseResult.exceptionString;
            }
            return "";
        }

        public class Received
        {
            public string android_received;

            public string ios_apns_sent;

            public long msg_id;
        }
    }
}