using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace cn.jpush.api.push.notification
{
    public abstract class PlatformNotification
    {
        public const string ALERT = "alert";

        private const string EXTRAS = "extras";

        public PlatformNotification()
        {
            alert = null;
            extras = null;
        }

        [JsonProperty]
        public string alert { get; protected set; }

        [JsonProperty]
        public Dictionary<string, object> extras { get; protected set; }
    }
}