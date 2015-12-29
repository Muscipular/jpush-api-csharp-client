using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using cn.jpush.api.common;
using Newtonsoft.Json;

namespace cn.jpush.api.device
{
    public class TagAliasResult : BaseResult
    {
        public string alias;

        public List<string> tags;

        public TagAliasResult()
        {
            tags = null;
            alias = null;
        }

        public override bool isResultOK()
        {
            return ResponseResult.responseCode == HttpStatusCode.OK;
        }

        public static TagAliasResult fromResponse(ResponseWrapper responseWrapper)
        {
            var tagAliasResult = new TagAliasResult();
            if (responseWrapper.isServerResponse())
            {
                tagAliasResult = JsonConvert.DeserializeObject<TagAliasResult>(responseWrapper.responseContent);
            }
            tagAliasResult.ResponseResult = responseWrapper;
            return tagAliasResult;
        }
    }
}