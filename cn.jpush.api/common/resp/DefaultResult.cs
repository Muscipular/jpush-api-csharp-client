using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace cn.jpush.api.common.resp
{
    public class DefaultResult : BaseResult
    {
        public static DefaultResult fromResponse(ResponseWrapper responseWrapper)
        {
            DefaultResult result = null;

            if (responseWrapper.isServerResponse())
            {
                result = new DefaultResult();
            }

            result.ResponseResult = responseWrapper;

            return result;
        }

        public override bool isResultOK()
        {
            if (ResponseResult.responseCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}