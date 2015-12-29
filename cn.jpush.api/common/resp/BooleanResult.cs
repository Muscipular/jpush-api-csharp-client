using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace cn.jpush.api.common.resp
{
    public class BooleanResult : DefaultResult
    {
        public bool result;

        public new static BooleanResult fromResponse(ResponseWrapper responseWrapper)
        {
            var tagListResult = new BooleanResult();
            if (responseWrapper.isServerResponse())
            {
                tagListResult = JsonConvert.DeserializeObject<BooleanResult>(responseWrapper.responseContent);
            }
            tagListResult.ResponseResult = responseWrapper;
            return tagListResult;
        }
    }
}