using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using cn.jpush.api.common;
using cn.jpush.api.util;
using Newtonsoft.Json;

namespace cn.jpush.api.push.mode
{
    public class Platform
    {
        private const string ALL = "all";

        private HashSet<string> _deviceTypes;

        private Platform()
        {
            allPlatform = ALL;
            deviceTypes = null;
        }

        private Platform(bool all, HashSet<string> deviceTypes)
        {
            //用来判断all=true时deviceTypes必须为空，反之当all=false时deviceTypes有值，不然json序列化会出错
            Debug.Assert(all && deviceTypes == null || !all && deviceTypes != null);
            if (all)
            {
                allPlatform = ALL;
            }
            this.deviceTypes = deviceTypes;
        }

        [JsonProperty(PropertyName = "winphone")]
        public string allPlatform { get; set; }

        public HashSet<string> deviceTypes
        {
            get { return _deviceTypes; }
            set
            {
                if (value != null)
                {
                    allPlatform = null;
                }
                _deviceTypes = value;
            }
        }

        public static Platform all()
        {
            return new Platform(true, null).Check();
        }

        public static Platform ios()
        {
            var types = new HashSet<string>();
            types.Add(DeviceType.ios.ToString());
            return new Platform(false, types).Check();
        }

        public static Platform android()
        {
            var types = new HashSet<string>();
            types.Add(DeviceType.android.ToString());
            return new Platform(false, types).Check();
        }

        public static Platform winphone()
        {
            var types = new HashSet<string>();
            types.Add(DeviceType.winphone.ToString());
            return new Platform(false, types).Check();
        }

        public static Platform android_ios()
        {
            var types = new HashSet<string>();
            types.Add(DeviceType.android.ToString());
            types.Add(DeviceType.ios.ToString());
            return new Platform(false, types).Check();
        }

        public static Platform android_winphone()
        {
            var types = new HashSet<string>();
            types.Add(DeviceType.android.ToString());
            types.Add(DeviceType.winphone.ToString());
            return new Platform(false, types).Check();
        }

        public static Platform ios_winphone()
        {
            var types = new HashSet<string>();
            types.Add(DeviceType.ios.ToString());
            types.Add(DeviceType.winphone.ToString());

            return new Platform(false, types).Check();
        }

        public bool isAll()
        {
            return allPlatform != null;
        }

        public void setAll(bool all)
        {
            if (all)
            {
                allPlatform = ALL;
            }
            else
            {
                allPlatform = null;
            }
        }

        public Platform Check()
        {
            Preconditions.checkArgument(!(isAll() && null != deviceTypes), "Since all is enabled, any platform should not be set.");
            Preconditions.checkArgument(!(!isAll() && null == deviceTypes), "No any deviceType is set.");
            return this;
        }
    }
}