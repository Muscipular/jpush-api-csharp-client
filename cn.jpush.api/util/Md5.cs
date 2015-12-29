using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace cn.jpush.api.util
{
    internal class Md5
    {
        public static string getMD5Hash(string str)
        {
            // MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(str), 0, str.Length);
            //char[] temp = new char[res.Length];
            //System.Array.Copy(res, temp, res.Length);
            //return new String(temp);


            // 创建MD5类的默认实例：MD5CryptoServiceProvider


            var md5 = MD5.Create();


            var bs = Encoding.UTF8.GetBytes(str);


            var hs = md5.ComputeHash(bs);


            var sb = new StringBuilder();


            foreach (var b in hs)
            {
                // 以十六进制格式格式化


                sb.Append(b.ToString("x2"));
            }


            return sb.ToString();
        }
    }
}