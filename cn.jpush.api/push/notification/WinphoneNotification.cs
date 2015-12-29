using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace cn.jpush.api.push.notification
{
    public class WinphoneNotification : PlatformNotification
    {
        [JsonProperty(PropertyName = "_open_page")]
        public string openPage;

        [JsonProperty]
        private string title;

        public WinphoneNotification()
        {
            title = null;
            openPage = null;
        }

        public WinphoneNotification setAlert(string alert)
        {
            this.alert = alert;
            return this;
        }

        public WinphoneNotification setOpenPage(string openPage)
        {
            this.openPage = openPage;
            return this;
        }

        public WinphoneNotification setTitle(string title)
        {
            this.title = title;
            return this;
        }

        public WinphoneNotification AddExtra(string key, string value)
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

        public WinphoneNotification AddExtra(string key, int value)
        {
            if (extras == null)
            {
                extras = new Dictionary<string, object>();
            }
            extras.Add(key, value);
            return this;
        }

        public WinphoneNotification AddExtra(string key, bool value)
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