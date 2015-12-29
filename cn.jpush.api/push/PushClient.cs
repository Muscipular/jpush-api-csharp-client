using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using cn.jpush.api.common;
using cn.jpush.api.push.mode;
using cn.jpush.api.util;
using Newtonsoft.Json;

namespace cn.jpush.api.push
{
    internal class PushClient : BaseHttpClient
    {
        private const string HOST_NAME_SSL = "https://api.jpush.cn";

        private const string PUSH_PATH = "/v3/push";

        private readonly string appKey;

        private readonly string masterSecret;

        public PushClient(string appKey, string masterSecret)
        {
            this.appKey = appKey;
            this.masterSecret = masterSecret;
        }

        public MessageResult sendPush(PushPayload payload)
        {
            Preconditions.checkArgument(payload != null, "pushPayload should not be empty");
            payload.Check();
            var payloadJson = payload.ToJson();
            return sendPush(payloadJson);
        }

        public MessageResult sendPush(string payloadString)
        {
            Preconditions.checkArgument(!string.IsNullOrEmpty(payloadString), "payloadString should not be empty");

            var url = HOST_NAME_SSL;
            url += PUSH_PATH;
            var result = sendPost(url, Authorization(), payloadString);
            var messResult = new MessageResult();
            messResult.ResponseResult = result;

            var jpushSuccess = JsonConvert.DeserializeObject<JpushSuccess>(result.responseContent);
            messResult.sendno = long.Parse(jpushSuccess.sendno);
            messResult.msg_id = long.Parse(jpushSuccess.msg_id);

            return messResult;
        }

        private string Authorization()
        {
            Debug.Assert(!string.IsNullOrEmpty(appKey));
            Debug.Assert(!string.IsNullOrEmpty(masterSecret));

            var origin = appKey + ":" + masterSecret;
            return Base64.getBase64Encode(origin);
        }
    }

    internal enum MsgTypeEnum
    {
        NOTIFICATIFY = 1,

        COUSTOM_MESSAGE = 2
    }
}