using System;
using System.Globalization;
using System.IO;
using System.Net;
using LyokoAPI.Events;
using Newtonsoft.Json;
using LyokoAPI.VirtualStructures.Interfaces;
using LyokoAPI.VirtualStructures;

namespace LyokoAPP_Plugin
{
    public static class DataPush
    {
        public static string SendData(ITower tower)
        {
            string dataKey = ServerKey.GetDataKey();

            try
            {
                var result = "-1";
                var time = DateTime.Now;
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                int unixDateTime = (int) (time.ToUniversalTime() - epoch).TotalSeconds;
                var webAddr = ("https://lyokoapp.firebaseio.com/" + Main.GetToken() + "/" + unixDateTime + ".json?auth=" + dataKey);
                
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.Method = "PATCH";
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    CultureInfo ci = CultureInfo.InstalledUICulture;
                    var sName = tower.Sector.Name;
                    FirstLetterToUpperCaseOrConvertNullToEmptyString(sName);
                    if (ci.TwoLetterISOLanguageName == "fr")
                    {
                        if (sName == "forest")
                        {
                            sName = "Forêt";
                        } else if (sName == "ice")
                        {
                            sName = "Banquise";
                        } else if (sName == "mountain")
                        {
                            sName = "Montagne";
                        } else if (sName == "desert")
                        {
                            sName = "Désert";
                        }
                    }
                    var data = new
                    {
                        sector = sName,
                        number = tower.Number.ToString(),
                        activator = Enum.GetName(typeof(APIActivator), tower.Activator)
                    };
                    var json = JsonConvert.SerializeObject(data);
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
                LyokoLogger.Log("LYOKOAPP", ex.ToString());
                return "SomethingWrong";
            }
        }
        
        
        public static string FirstLetterToUpperCaseOrConvertNullToEmptyString(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        
    }
}