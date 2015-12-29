using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using cn.jpush.api.common;
using Newtonsoft.Json;

namespace cn.jpush.api.report
{
    public class UsersResult : BaseResult
    {
        public int duration;

        public List<User> items = new List<User>();

        public string start;

        public TimeUnit time_unit;

        public UsersResult()
        {
            time_unit = TimeUnit.DAY;
            start = null;
            duration = 0;
        }

        public static UsersResult fromResponse(ResponseWrapper responseWrapper)
        {
            var usersResult = new UsersResult();
            if (responseWrapper.isServerResponse())
            {
                usersResult = JsonConvert.DeserializeObject<UsersResult>(responseWrapper.responseContent);
            }
            usersResult.ResponseResult = responseWrapper;
            return usersResult;
        }

        public override bool isResultOK()
        {
            return ResponseResult.responseCode == HttpStatusCode.OK;
        }

        public class User
        {
            public Android android;

            public Ios ios;

            public string time;

            public User()
            {
                time = null;
                android = null;
                ios = null;
            }
        }

        public class Android
        {
            public int active;

            [JsonProperty(PropertyName = "new")]
            public long add;

            public int online;

            public Android()
            {
                add = 0;
                online = 0;
                active = 0;
            }
        }

        public class Ios
        {
            public int active;

            [JsonProperty(PropertyName = "new")]
            public long add;

            public int online;

            public Ios()
            {
                add = 0;
                online = 0;
                active = 0;
            }
        }
    }
}