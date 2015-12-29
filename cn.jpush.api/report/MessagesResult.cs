using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using cn.jpush.api.common;
using Newtonsoft.Json;

namespace cn.jpush.api.report
{
    public class MessagesResult : BaseResult
    {
        public List<Message> messages = new List<Message>();

        public static MessagesResult fromResponse(ResponseWrapper responseWrapper)
        {
            var receivedsResult = new MessagesResult();
            if (responseWrapper.responseCode == HttpStatusCode.OK)
            {
                receivedsResult.messages = JsonConvert.DeserializeObject<List<Message>>(responseWrapper.responseContent);
            }
            receivedsResult.ResponseResult = responseWrapper;
            return receivedsResult;
        }

        public override bool isResultOK()
        {
            return ResponseResult.responseCode == HttpStatusCode.OK;
        }

        public class Message
        {
            public Android android;

            public Ios ios;

            public long msg_id;

            public Message()
            {
                msg_id = 0;
                android = null;
                ios = null;
            }
        }

        public class Android
        {
            public int click;

            public int online_push;

            public int received;

            public int target;

            public Android()
            {
                received = 0;
                target = 0;
                online_push = 0;
                click = 0;
            }
        }

        public class Ios
        {
            public int apns_sent;

            public int apns_target;

            public int click;

            public Ios()
            {
                apns_sent = 0;
                apns_target = 0;
                click = 0;
            }
        }
    }
}