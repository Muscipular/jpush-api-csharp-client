using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using cn.jpush.api.report;

namespace cn.jpush.api.util
{
    public class JsonTool
    {
        // 从一个对象信息生成Json串        
        public static string ObjectToJson(object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            var stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            var dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes).Replace("\\", "");
        }

        // 从一个Json串生成对象信息        
        public static object JsonToObject(string jsonString, object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            var mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serializer.ReadObject(mStream);
        }

        // 从一个对象信息生成Json串        
        public static string DictionaryToJson(Dictionary<string, object> dict)
        {
            var json = new StringBuilder();

            foreach (var pair in dict)
            {
                json.Append("\"").Append(pair.Key).Append("\"").Append(":").Append(ValueToJson(pair.Value)).Append(",");
            }
            //Console.WriteLine("json String ******"+json);
            if (json.Length > 0)
            {
                json.Remove(json.Length - 1, 1);
            }
            json.Append("}");
            json.Insert(0, "{");

            return json.ToString();
        }

        public static List<ReceivedResult.Received> JsonList(string jsonString)
        {
            var Serializer = new JavaScriptSerializer();
            var jsonclassList = Serializer.Deserialize<List<ReceivedResult.Received>>(jsonString);
            return jsonclassList;
        }

        //从dictionary 的value中解析出字符串
        private static string ValueToJson(object value)
        {
            var type = value.GetType();
            if (type == typeof(int))
            {
                return value.ToString();
            }
            if (type == typeof(string))
            {
                return "\"" + value + "\"";
            }
            if (type == typeof(List<int>) || type == typeof(List<string>))
            {
                return ObjectToJson(value);
            }
            if (type == typeof(Dictionary<string, object>))
            {
                return DictionaryToJson((Dictionary<string, object>)value);
            }
            Debug.WriteLine("Type in Dictionary is error!");
            return "type erro";
        }
    }
}