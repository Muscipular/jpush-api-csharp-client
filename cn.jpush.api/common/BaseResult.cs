using System;
using System.Collections.Generic;
using System.Linq;

namespace cn.jpush.api.common
{
    public abstract class BaseResult
    {
        public const int ERROR_CODE_NONE = -1;

        public const int ERROR_CODE_OK = 0;

        public const string ERROR_MESSAGE_NONE = "None error message.";

        public const int RESPONSE_OK = 200;

        public ResponseWrapper ResponseResult { get; set; }

        public abstract bool isResultOK();


        // public override String getErrorMessage();

        public int getRateLimitQuota()
        {
            if (null != ResponseResult)
            {
                return ResponseResult.rateLimitQuota;
            }
            return 0;
        }

        public int getRateLimitRemaining()
        {
            if (null != ResponseResult)
            {
                return ResponseResult.rateLimitRemaining;
            }
            return 0;
        }

        public int getRateLimitReset()
        {
            if (null != ResponseResult)
            {
                return ResponseResult.rateLimitReset;
            }
            return 0;
        }
    }
}