using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using cn.jpush.api.common;
using cn.jpush.api.util;
using Newtonsoft.Json;

namespace cn.jpush.api.report
{
    internal class ReportClient : BaseHttpClient
    {
        private const string REPORT_HOST_NAME = "https://report.jpush.cn";

        private const string REPORT_RECEIVE_PATH = "/v2/received";

        private const string REPORT_RECEIVE_PATH_V3 = "/v3/received";

        private const string REPORT_USER_PATH = "/v3/users";

        private readonly string appKey;

        private readonly string masterSecret;

        public ReportClient(string appKey, string masterSecret)
        {
            this.appKey = appKey;
            this.masterSecret = masterSecret;
        }

        public ReceivedResult getReceiveds(string msg_ids)
        {
            checkMsgids(msg_ids);
            return getReceiveds_common(msg_ids, REPORT_RECEIVE_PATH);
        }

        public ReceivedResult getReceiveds_v3(string msg_ids)
        {
            checkMsgids(msg_ids);
            return getReceiveds_common(msg_ids, REPORT_RECEIVE_PATH_V3);
        }

        public UsersResult getUsers(TimeUnit timeUnit, string start, int duration)
        {
            var url = string.Format("{0}{1}?time_unit={2}&start={3}&duration={4}", REPORT_HOST_NAME, REPORT_USER_PATH, timeUnit, start, duration);
            var auth = Base64.getBase64Encode(appKey + ":" + masterSecret);
            var response = sendGet(url, auth, null);

            return UsersResult.fromResponse(response);
        }

        public MessagesResult getReportMessages(params string[] msgIds)
        {
            return getReportMessages(StringUtil.arrayToString(msgIds));
        }

        private string checkMsgids(string msgIds)
        {
            if (string.IsNullOrEmpty(msgIds))
            {
                throw new ArgumentException("msgIds param is required.");
            }
            var reg = new Regex(@"[^0-9, ]");
            if (reg.IsMatch(msgIds))
            {
                throw new ArgumentException("msgIds param format is incorrect. "
                                            + "It should be msg_id (number) which response from JPush Push API. "
                                            + "If there are many, use ',' as interval. ");
            }
            msgIds = msgIds.Trim();
            if (msgIds.EndsWith(","))
            {
                msgIds = msgIds.Substring(0, msgIds.Length - 1);
            }
            var splits = msgIds.Split(',');
            var list = new List<string>(splits.Length);
            foreach (var s in splits)
            {
                var trim = s.Trim();
                if (!string.IsNullOrEmpty(trim))
                {
                    long idVal;
                    if (!long.TryParse(trim, out idVal))
                    {
                        throw new Exception("Every msg_id should be valid Integer number which splits by ','");
                    }
                    list.Add(trim);
                }
            }
            return StringUtil.arrayToString(list.ToArray());
        }

        private ReceivedResult getReceiveds_common(string msg_ids, string path)
        {
            var url = string.Format("{0}{1}?msg_ids={2}", REPORT_HOST_NAME, path, msg_ids);
            var auth = Base64.getBase64Encode(appKey + ":" + masterSecret);
            var rsp = sendGet(url, auth, null);
            var result = new ReceivedResult();
            var list = new List<ReceivedResult.Received>();

            Debug.WriteLine("recieve content==" + rsp.responseContent);
            if (rsp.responseCode == HttpStatusCode.OK)
            {
                list = JsonConvert.DeserializeObject<List<ReceivedResult.Received>>(rsp.responseContent); //(List<ReceivedResult.Received>)JsonTool.JsonToObject(rsp.responseContent, list);
                var content = rsp.responseContent;
            }
            result.ResponseResult = rsp;
            result.ReceivedList = list;
            return result;
        }

        private MessagesResult getReportMessages(string msgIds)
        {
            var checkMsgId = checkMsgids(msgIds);
            var url = string.Format("{0}{1}?msg_ids={2}", REPORT_HOST_NAME, REPORT_RECEIVE_PATH, checkMsgId);
            var auth = Base64.getBase64Encode(appKey + ":" + masterSecret);
            var response = sendGet(url, auth, null);

            return MessagesResult.fromResponse(response);
        }
    }
}