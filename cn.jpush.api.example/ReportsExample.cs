using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cn.jpush.api.example
{
    class ReportsExample
    {
         public static String app_key = "997f28c1cea5a9f17d82079a";
         public static String master_secret = "47d264a3c02a6a5a4a256a45";

         public static void testGetReport()
         {
             JPushClient jpushClient = new JPushClient(app_key, master_secret);
             try
             {
                 var result = jpushClient.getReceivedApi("1942377665");
                 Debug.WriteLine("Got result - " + result.ToString());

             }
             catch (APIRequestException e)
             {
                 Debug.WriteLine("Error response from JPush server. Should review and fix it. ");
                 Debug.WriteLine("HTTP Status: " + e.Status);
                 Debug.WriteLine("Error Code: " + e.ErrorCode);
                 Debug.WriteLine("Error Message: " + e.ErrorCode);
             }
             catch (APIConnectionException e)
             {
                 Debug.WriteLine(e.Message);
             }
         }
         /*用户统计vip专用接口*/
         public static void testGetUsers()
         {
             JPushClient jpushClient = new JPushClient(app_key, master_secret);
             try
             {
                 var result = jpushClient.getReportUsers(TimeUnit.DAY, "2014-06-10", 3);
                 Debug.WriteLine("Got result - " + result.ToString());

             }
             catch (APIRequestException e)
             {
                 Debug.WriteLine("Error response from JPush server. Should review and fix it. ");
                 Debug.WriteLine("HTTP Status: " + e.Status);
                 Debug.WriteLine("Error Code: " + e.ErrorCode);
                 Debug.WriteLine("Error Message: " + e.ErrorCode);
             }
             catch (APIConnectionException e)
             {
                 Debug.WriteLine(e.Message);
             }
         }
         /*消息统计vip专用接口*/
         public static void testGetMessages()
         {
             JPushClient jpushClient = new JPushClient(app_key, master_secret);
             try
             {
                 var result = jpushClient.getReportMessages("269978303");
                 Debug.WriteLine("Got result - " + result.ToString());

             }
             catch (APIRequestException e)
             {
                 Debug.WriteLine("Error response from JPush server. Should review and fix it. ");
                 Debug.WriteLine("HTTP Status: " + e.Status);
                 Debug.WriteLine("Error Code: " + e.ErrorCode);
                 Debug.WriteLine("Error Message: " + e.ErrorCode);
             }
             catch (APIConnectionException e)
             {
                 Debug.WriteLine(e.Message);
             }

         }

    }
}
