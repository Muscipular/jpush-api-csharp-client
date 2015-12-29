using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.util;
using Newtonsoft.Json.Linq;

namespace cn.jpush.api.device
{
    internal class DeviceClient : BaseHttpClient
    {
        public const string HOST_NAME_SSL = "https://device.jpush.cn";

        public const string DEVICES_PATH = "/v3/devices";

        public const string TAGS_PATH = "/v3/tags";

        public const string ALIASES_PATH = "/v3/aliases";

        private readonly string appKey;

        private readonly string masterSecret;

        public DeviceClient(string appKey, string masterSecret)
        {
            this.appKey = appKey;
            this.masterSecret = masterSecret;
        }

        public TagAliasResult getDeviceTagAlias(string registrationId)
        {
            var url = string.Format("{0}{1}/{2}", HOST_NAME_SSL, DEVICES_PATH, registrationId);
            var auth = Base64.getBase64Encode(appKey + ":" + masterSecret);

            var response = sendGet(url, auth, null);

            return TagAliasResult.fromResponse(response);
        }

        public DefaultResult updateDeviceTagAlias(string registrationId, bool clearAlias, bool clearTag)
        {
            Preconditions.checkArgument(clearAlias || clearTag, "It is not meaningful to do nothing.");

            var url = string.Format("{0}{1}/{2}", HOST_NAME_SSL, DEVICES_PATH, registrationId);

            var top = new JObject();
            if (clearAlias)
            {
                top.Add("alias", "");
            }
            if (clearTag)
            {
                top.Add("tags", "");
            }
            var result = sendPost(url, Authorization(), top.ToString());

            return DefaultResult.fromResponse(result);
        }

        public DefaultResult updateDeviceTagAlias(string registrationId,
                                                  string alias,
                                                  HashSet<string> tagsToAdd,
                                                  HashSet<string> tagsToRemove)
        {
            var url = string.Format("{0}{1}/{2}", HOST_NAME_SSL, DEVICES_PATH, registrationId);

            var top = new JObject();
            if (null != alias)
            {
                top.Add("alias", alias);
            }

            var tagObject = new JObject();
            if (tagsToAdd != null)
            {
                var tagsAdd = JArray.FromObject(tagsToAdd);
                if (tagsAdd.Count > 0)
                {
                    tagObject.Add("add", tagsAdd);
                }
            }
            if (tagsToRemove != null)
            {
                var tagsRemove = JArray.FromObject(tagsToRemove);
                if (tagsRemove.Count > 0)
                {
                    tagObject.Add("remove", tagsRemove);
                }
            }

            if (tagObject.Count > 0)
            {
                top.Add("tags", tagObject);
            }
            var result = sendPost(url, Authorization(), top.ToString());

            return DefaultResult.fromResponse(result);
        }

        public TagListResult getTagList()
        {
            var url = string.Format("{0}{1}/", HOST_NAME_SSL, TAGS_PATH);
            var auth = Base64.getBase64Encode(appKey + ":" + masterSecret);
            var response = sendGet(url, auth, null);
            return TagListResult.fromResponse(response);
        }

        public BooleanResult isDeviceInTag(string theTag, string registrationID)
        {
            var url = string.Format("{0}{1}/{2}/registration_ids/{3}", HOST_NAME_SSL, TAGS_PATH, theTag, registrationID);
            var response = sendGet(url, Authorization(), null);
            return BooleanResult.fromResponse(response);
        }

        public DefaultResult addRemoveDevicesFromTag(string theTag,
                                                     HashSet<string> toAddUsers,
                                                     HashSet<string> toRemoveUsers)
        {
            var url = string.Format("{0}{1}/{2}", HOST_NAME_SSL, TAGS_PATH, theTag);

            var top = new JObject();
            var registrationIds = new JObject();
            if (null != toAddUsers && toAddUsers.Count > 0)
            {
                var array = new JArray();
                foreach (var user in toAddUsers)
                {
                    array.Add(JToken.FromObject(user));
                }
                registrationIds.Add("add", array);
            }
            if (null != toRemoveUsers && toRemoveUsers.Count > 0)
            {
                var array = new JArray();
                foreach (var user in toRemoveUsers)
                {
                    array.Add(JToken.FromObject(user));
                }
                registrationIds.Add("remove", array);
            }
            top.Add("registration_ids", registrationIds);
            var response = sendPost(url, Authorization(), top.ToString());
            return DefaultResult.fromResponse(response);
        }

        public DefaultResult deleteTag(string theTag, string platform)
        {
            var url = string.Format("{0}{1}/{2}", HOST_NAME_SSL, TAGS_PATH, theTag);
            if (null != platform)
            {
                url += "?platform=" + platform;
            }

            var response = sendDelete(url, Authorization(), null);
            return DefaultResult.fromResponse(response);
        }

        // ------------- alias

        public AliasDeviceListResult getAliasDeviceList(string alias, string platform)
        {
            var url = string.Format("{0}{1}/{2}", HOST_NAME_SSL, ALIASES_PATH, alias);
            if (null != platform)
            {
                url += "?platform=" + platform;
            }
            var response = sendGet(url, Authorization(), null);

            return AliasDeviceListResult.fromResponse(response);
        }

        public DefaultResult deleteAlias(string alias, string platform)
        {
            var url = string.Format("{0}{1}/{2}", HOST_NAME_SSL, ALIASES_PATH, alias);
            if (null != platform)
            {
                url += "?platform=" + platform;
            }
            var response = sendDelete(url, Authorization(), null);
            return DefaultResult.fromResponse(response);
        }

        private string Authorization()
        {
            Debug.Assert(!string.IsNullOrEmpty(appKey));
            Debug.Assert(!string.IsNullOrEmpty(masterSecret));
            var origin = appKey + ":" + masterSecret;
            return Base64.getBase64Encode(origin);
        }
    }
}