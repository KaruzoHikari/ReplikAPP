using System;
using System.IO;
using System.Net;

namespace LyokoAPP_Plugin
{
    public class FireBasePush
    {
        public static string SendMessage(string title, string body)
        {
            string serverKey = ServerKey.GetServerKey();
 
            try
            {
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";
 
                var regID  = Main.GetToken();
 
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Method = "POST";
 
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"to\": \"" + regID + "\",\"notification\": {\"title\": \"" + title + "\",\"body\": \"" + body + "\"},\"priority\":10}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
 
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
 
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "SomethingWrong";
            }
        }
    }
}