using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using cn.jpush.api.common;
using Newtonsoft.Json;

namespace cn.jpush.api.device
{
    public class TagListResult : BaseResult
    {
        public List<string> tags;

        public TagListResult()
        {
            tags = null;
        }

        public override bool isResultOK()
        {
            return ResponseResult.responseCode == HttpStatusCode.OK;
        }

        public static TagListResult fromResponse(ResponseWrapper responseWrapper)
        {
            var tagListResult = new TagListResult();
            if (responseWrapper.isServerResponse())
            {
                tagListResult = JsonConvert.DeserializeObject<TagListResult>(responseWrapper.responseContent);
            }
            tagListResult.ResponseResult = responseWrapper;
            return tagListResult;
        }
    }
}