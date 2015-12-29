using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using cn.jpush.api.common;
using cn.jpush.api.util;
using Newtonsoft.Json;

namespace cn.jpush.api.push.mode
{
    public class PushPayload
    {
        private const string PLATFORM = "platform";

        private const string AUDIENCE = "audience";

        private const string NOTIFICATION = "notification";

        private const string MESSAGE = "message";

        private const string OPTIONS = "options";

        private const int MAX_GLOBAL_ENTITY_LENGTH = 1200; // Definition acording to JPush Docs

        private const int MAX_IOS_PAYLOAD_LENGTH = 220; // Definition acording to JPush Docs

        private readonly JsonSerializerSettings jSetting;

        //construct
        public PushPayload()
        {
            platform = null;
            audience = null;
            notification = null;
            message = null;
            options = new Options();
            jSetting = new JsonSerializerSettings();
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            jSetting.DefaultValueHandling = DefaultValueHandling.Ignore;
        }

        public PushPayload(Platform platform, Audience audience, Notification notification, Message message = null, Options options = null)
        {
            Debug.Assert(platform != null);
            Debug.Assert(audience != null);
            Debug.Assert(notification != null || message != null);

            this.platform = platform;
            this.audience = audience;
            this.notification = notification;
            this.message = message;
            this.options = options;

            jSetting = new JsonSerializerSettings();
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            jSetting.DefaultValueHandling = DefaultValueHandling.Ignore;
        }

        //serializaiton property
        [JsonConverter(typeof(PlatformConverter))]
        public Platform platform { get; set; }

        [JsonConverter(typeof(AudienceConverter))]
        public Audience audience { get; set; }

        public Notification notification { get; set; }

        public Message message { get; set; }

        public Options options { get; set; }

        /**
         * The shortcut of building a simple alert notification object to all platforms and all audiences
        */

        public static PushPayload AlertAll(string alert)
        {
            return new PushPayload(Platform.all(),
                Audience.all(),
                new Notification().setAlert(alert),
                null,
                new Options());
        }

        //* The shortcut of building a simple message object to all platforms and all audiences
        //*/
        public static PushPayload MessageAll(string msgContent)
        {
            return new PushPayload(Platform.all(),
                Audience.all(),
                null,
                Message.content(msgContent),
                new Options());
        }

        public static PushPayload FromJSON(string payloadString)
        {
            try
            {
                var jSetting = new JsonSerializerSettings();
                jSetting.NullValueHandling = NullValueHandling.Ignore;
                jSetting.DefaultValueHandling = DefaultValueHandling.Ignore;

                var jsonObject = JsonConvert.DeserializeObject<PushPayload>(payloadString, jSetting);
                return jsonObject.Check();
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON to PushPayLoad occur error:" + e.Message);
                return null;
            }
        }

        public void ResetOptionsApnsProduction(bool apnsProduction)
        {
            if (options == null)
            {
                options = new Options();
            }
            options.apns_production = apnsProduction;
        }

        public void ResetOptionsTimeToLive(long timeToLive)
        {
            if (options == null)
            {
                options = new Options();
            }
            options.time_to_live = timeToLive;
        }

        public int GetSendno()
        {
            if (options != null)
            {
                return options.sendno;
            }
            return 0;
        }

        public bool IsGlobalExceedLength()
        {
            var messageLength = 0;
            if (message != null)
            {
                var messageJson = JsonConvert.SerializeObject(message, jSetting);
                messageLength += Encoding.UTF8.GetBytes(messageJson).Length;
            }
            if (notification == null)
            {
                return messageLength > MAX_GLOBAL_ENTITY_LENGTH;
            }
            var notificationJson = JsonConvert.SerializeObject(notification);
            if (notificationJson != null)
            {
                messageLength += Encoding.UTF8.GetBytes(notificationJson).Length;
            }
            return messageLength > MAX_GLOBAL_ENTITY_LENGTH;
        }

        public bool IsIosExceedLength()
        {
            if (notification != null)
            {
                if (notification.IosNotification != null)
                {
                    var iosJson = JsonConvert.SerializeObject(notification.IosNotification, jSetting);
                    if (iosJson != null)
                    {
                        return Encoding.UTF8.GetBytes(iosJson).Length > MAX_IOS_PAYLOAD_LENGTH;
                    }
                }
                else
                {
                    if (!(notification.alert == null))
                    {
                        string jsonText;
                        using (var sw = new StringWriter())
                        {
                            JsonWriter writer = new JsonTextWriter(sw);
                            writer.WriteValue(notification.alert);
                            writer.Flush();
                            jsonText = sw.GetStringBuilder().ToString();
                        }
                        return Encoding.UTF8.GetBytes(jsonText).Length > MAX_IOS_PAYLOAD_LENGTH;
                    }
                }
            }
            return false;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, jSetting);
        }

        public PushPayload Check()
        {
            Preconditions.checkArgument(!(null == audience || null == platform), "audience and platform both should be set.");
            Preconditions.checkArgument(!(null == notification && null == message), "notification or message should be set at least one.");
            if (audience != null)
            {
                audience.Check();
            }
            if (platform != null)
            {
                platform.Check();
            }
            if (message != null)
            {
                message.Check();
            }
            if (notification != null)
            {
                notification.Check();
            }
            return this;
        }
    }
}