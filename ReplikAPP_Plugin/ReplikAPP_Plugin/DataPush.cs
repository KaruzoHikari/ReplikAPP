using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LyokoAPI.Events;
using LyokoAPI.VirtualStructures;
using LyokoAPI.VirtualStructures.Interfaces;
using Newtonsoft.Json;

//using System.Threading.Tasks;

namespace ReplikAPP_Plugin
{
    public static class DataPush
    {
        static DataPush()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true; 
        }
        public static void SendData(ITower tower)
        {
            Task.Run(() =>
            {
                try
                {


                    string dataKey = ServerKey.GetDataKey();

                    var result = "-1";
                    var time = DateTime.Now;
                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    int unixDateTime = (int) (time.ToUniversalTime() - epoch).TotalSeconds;
                    var webAddr = ("https://lyokoapp.firebaseio.com/" + Main.GetToken() + "/" + unixDateTime +
                                   ".json?auth=" + dataKey);

                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(webAddr);
                    httpWebRequest.Method = "PATCH";
                    httpWebRequest.ContentType = "application/json; charset=utf-8";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var sName = Main.GetUppercaseNames(tower.Sector.Name);
                        /*string json = "{\"world\": \"" + tower.Sector.World.Name + "\",\"sector\": \"" + sName +
                                      "\",\"number\": \"" +
                                      tower.Number.ToString() + "\",\"activator\": \"" +
                                      Enum.GetName(typeof(APIActivator), tower.Activator) + "\"}";*/
                        var objectJson = new
                        {
                            world = tower.Sector.World.Name,
                            sector = sName,
                            number = tower.Number,
                            activator = Enum.GetName(typeof(APIActivator), tower.Activator)
                        };
                        string json2 = JsonConvert.SerializeObject(objectJson);
                        //LyokoLogger.Log("REPL", json2);
                        streamWriter.Write(json2);
                        streamWriter.Flush();
                    }

                    var httpResponse = (HttpWebResponse) httpWebRequest.GetResponseNoException();
                    using (var streamReader =
                        new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    //LyokoLogger.Log("REPL",result);
                    //LyokoLogger.Log("REPL",httpResponse.GetResponseHeader("google.rpc.BadRequest"));
                    return result;
                        
                      
                }  catch (Exception e)
                {
                    LyokoLogger.Log("ReplikAPP", e.Message + e.StackTrace);
                }

                return "-1";
            });
        }
    }
}