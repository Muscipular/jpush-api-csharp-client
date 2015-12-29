using System;
using System.Collections.Generic;
using System.Linq;

namespace cn.jpush.api.common.resp
{
    public class APIConnectionException : Exception
    {
        public APIConnectionException(string message) : base(message)
        {
        }
    }
}