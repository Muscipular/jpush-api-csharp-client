using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cn.jpush.api.util
{
    internal class Base64
    {
        public static string getBase64Encode(string str)
        {
            var bytes = Encoding.Default.GetBytes(str);
            //
            return Convert.ToBase64String(bytes);
        }
    }
}