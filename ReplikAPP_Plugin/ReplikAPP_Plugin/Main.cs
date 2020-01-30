using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using LyokoAPI.Events;
using LyokoAPI.Plugin;
using LyokoAPI.VirtualStructures;
using YamlDotNet.Core.Tokens;

namespace ReplikAPP_Plugin
{
    public class Main : LyokoAPIPlugin
    {
        public override string Name { get; } = "ReplikAPP";
        public override string Author { get; } = "KaruzoHikari and Appryl";
        public string Version = "1.0";
        private static string _pin = "";
        private static string _token = "";

        private static Dictionary<string,string> localization = new Dictionary<string, string>();

        protected override bool OnEnable()
        {
            if (!ReadPin())
            {
                return false;
            }

            createLocalizationDictionary();
            Listener.StartListening();
            return true;
        }

        protected override bool OnDisable()
        {
            Listener.StopListening();
            return true;
        }
        
        public override void OnGameStart(bool storyMode)
        {
            //nothing
        }

        public override void OnGameEnd(bool failed)
        {
            //nothing again Jack why do you make us fill these methods
        }

        private bool ReadPin()
        {
            string directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + @"\ReplikAPP";
            string fileDirectory = directory + @"\UserPin.txt";
            string oldDirectory = directory + @"\UserToken.txt";
            if (Directory.Exists(directory) && File.Exists(oldDirectory))
            {
                File.Delete(oldDirectory);
            }
            if (Directory.Exists(directory) && File.Exists(fileDirectory))
            {
                try
                {
                    string pin = File.ReadAllText(fileDirectory, Encoding.UTF8);
                    if (pin.Length > 0 && pin.Length < 7)
                    {
                        _pin = pin.Trim();
                        LyokoLogger.Log(Name, "User pin successfully loaded!");
                        checkVersion();
                        _token = FetchToken();
                        return true;
                    }

                    LyokoLogger.Log(Name, @"Please input the pin given by ReplikAPP in \ReplikAPP\UserPin.txt");
                    LyokoLogger.Log(Name, "The plugin will disable now, restart the game once the pin is in the file.");
                    return false;
                }
                catch (Exception e)
                {
                    LyokoLogger.Log(Name,
                        "Something went wrong when reading the pin! Please check that only your pin is inside UserPin.txt");
                    return false;
                }
            }

            try
            {
                Directory.CreateDirectory(directory);
                File.Create(fileDirectory);
            }
            catch (Exception e)
            {
                LyokoLogger.Log(Name,
                    $"Something went wrong creating the config directory: {e.ToString()}, check if you have write access to the directory");
                return false;
            }

            LyokoLogger.Log(Name,
                @"Please input the pin given by ReplikAPP in UserPin.txt (in ReplikAPP's folder)");
            LyokoLogger.Log(Name, "The plugin will disable now, restart the game once the pin is in the file.");
            return false;
        }

        public static string GetPin()
        {
            return _pin;
        }

        public static string FetchToken()
        {
            string dataKey = ServerKey.GetDataKey();
            string token = "void";
            var webAddr = ("https://lyokoapp.firebaseio.com/-USERS/" + _pin + "/-TOKEN.json?auth=" + dataKey);
            using (var webClient = new System.Net.WebClient()) {
                
                var result = webClient.DownloadString(webAddr);
                Dictionary<string, object> dictionary = JsonParser.ParseJSON(result);
                foreach (string key in dictionary.Keys)
                {
                    token = key;
                    Console.WriteLine(token);
                }

            }
            return token;
        }
        
        public static string GetToken()
        {
            return _token;
        }

        public static string GetUppercaseNames(string name)
        {
            if (name.Length < 1 || name.Equals("XANA", StringComparison.OrdinalIgnoreCase))
            {
                return name;
            }
            if (name.Equals("carthage", StringComparison.OrdinalIgnoreCase))
            {
                return "Sector 5";
            }

            return char.ToUpperInvariant(name[0]) + name.Substring(1).ToLowerInvariant();
        }

        private void createLocalizationDictionary()
        {
            try
            {
                string langCode = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = "ReplikAPP_Plugin.Localization.ReplikAPP_Plugin_" + langCode.ToUpperInvariant() + ".json";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    Dictionary<string, object> dictionary = JsonParser.ParseJSON(result);
                    foreach (string key in dictionary.Keys)
                    {
                        
                        localization.Add(cleanString(key), cleanString((string) dictionary[key],true));
                    }
                }
            }
            catch (Exception e)
            {
                LyokoLogger.Log("ReplikAPP",e.ToString());
            }
        }

        private string cleanString(string check, bool allowWhiteSpaces = false)
        {
            /*char[] arr = check.ToCharArray();
            if (!allowWhiteSpaces)
            {
                arr = Array.FindAll(arr, (c => (char.IsLetterOrDigit(c) || c == '.')));
            }
            else
            {
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '.')));
            }

            return new string(arr).Trim();*/
            return check.Trim();
        }

        public static string localize(string text)
        {
            if(localization.Keys.Contains(text))
            {
                return localization[text];
            }
            return text;
        }
        
        private void checkVersion()
        {
            string dataKey = ServerKey.GetDataKey();
            
            var webAddr = ("https://lyokoapp.firebaseio.com/-VERSION.json?auth=" + dataKey);
            using (var webClient = new System.Net.WebClient()) {
                
                var result = webClient.DownloadString(webAddr);
                if (result != "\"" + Version + "\"")
                {
                    FireBasePush.SendMessage(localize("versionCheck.title"), localize("versionCheck.body"));
                }
                
            }
        }
    }
}