using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace cn.jpush.api.push.notification
{
    public class AndroidNotification : PlatformNotification
    {
        public const string NOTIFICATION_ANDROID = "android";

        private const string TITLE = "title";

        private const string BUILDER_ID = "builder_id";

        public AndroidNotification()
        {
            title = null;
            builder_id = 0;
        }

        [JsonProperty]
        public string title { get; private set; }

        [JsonProperty]
        public int builder_id { get; private set; }

        public AndroidNotification setTitle(string title)
        {
            this.title = title;
            return this;
        }

        public AndroidNotification setBuilderID(int builder_id)
        {
            this.builder_id = builder_id;
            return this;
        }

        public AndroidNotification setAlert(string alert)
        {
            this.alert = alert;
            return this;
        }

        public AndroidNotification AddExtra(string key, string value)
        {
            if (extras == null)
            {
                extras = new Dictionary<string, object>();
            }
            if (value != null)
            {
                extras.Add(key, value);
            }
            return this;
        }

        public AndroidNotification AddExtra(string key, int value)
        {
            if (extras == null)
            {
                extras = new Dictionary<string, object>();
            }
            extras.Add(key, value);
            return this;
        }

        public AndroidNotification AddExtra(string key, bool value)
        {
            if (extras == null)
            {
                extras = new Dictionary<string, object>();
            }
            extras.Add(key, value);
            return this;
        }
    }
}