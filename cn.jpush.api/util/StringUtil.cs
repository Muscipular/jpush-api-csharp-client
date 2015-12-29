using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace cn.jpush.api.util
{
    internal class StringUtil
    {
//        public bool IsNumber(string strNumber)
//        {
//            var objNotNumberPattern = new Regex("[^0-9.-]");
//            var objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
//            var objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
//            var strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
//            var strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
//            var objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
//
//            return !objNotNumberPattern.IsMatch(strNumber) &&
//                   !objTwoDotPattern.IsMatch(strNumber) &&
//                   !objTwoMinusPattern.IsMatch(strNumber) &&
//                   objNumberPattern.IsMatch(strNumber);
//        }
//
//        public static bool IsNumeric(string value)
//        {
//            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
//        }
//
//        public static bool IsInt(string value)
//        {
//            return Regex.IsMatch(value, @"^[+-]?\d*$");
//        }
//
//        public static bool IsUnsign(string value)
//        {
//            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
//        }

        public static string arrayToString(string[] values)
        {
            if (null == values || values.Length == 0)
            {
                return "";
            }

            var buffer = new StringBuilder(values.Length);
            for (var i = 0; i < values.Length; i++)
            {
                buffer.Append(values[i]).Append(",");
            }
            if (buffer.Length > 0)
            {
                return buffer.ToString(0, buffer.Length - 1);
            }
            return "";
        }
    }
}