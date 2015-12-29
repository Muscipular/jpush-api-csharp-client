using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace cn.jpush.api.common
{
    public enum DeviceType
    {
        [Description("android")]
        android,

        [Description("ios")]
        ios,

        [Description("winphone")]
        winphone
    }
}