using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
                    //READ PIN
                    string pin = File.ReadAllText(fileDirectory, Encoding.UTF8).Trim();
                    if (pin.Length > 0 && pin.Length < 7)
                    {
                        _pin = pin;
                        LyokoLogger.Log(Name, "User pin successfully loaded!");
                        checkVersion();
                        _token = FetchToken();
                        Listener.StartListening();
                        return true;
                    }
                    
                    //IF IT DOESN'T EXIST, ASK FOR IT
                    string batFileName = Path.GetTempPath() + @"\pin.bat";
                    string programName = Assembly.GetExecutingAssembly().GetName().Name;

                    using (Stream input = Assembly.GetExecutingAssembly().GetManifestResourceStream(programName + ".pin.bat"))
                    {
                        using (TextReader tr = new StreamReader(input))
                        {
                            File.WriteAllText(batFileName, tr.ReadToEnd());
                        }
                    }

                    Process proc = new Process();
                    proc.EnableRaisingEvents = true;
                    proc.StartInfo.CreateNoWindow = false;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.FileName = "cmd.exe";
                    proc.StartInfo.Arguments = "/c " + batFileName;
                    
                    proc.Start();
                    proc.Exited += (sender, e) =>
                    {
                        try
                        {
                            LyokoLogger.Log(Name, "PIN window closed!");
                            string path = Path.GetTempPath() + @"\UserPin.txt";
                            if (File.Exists(path))
                            {
                                if (File.Exists(fileDirectory))
                                {
                                    File.Delete(fileDirectory);
                                }
                                File.Move(path, fileDirectory);
                                string newPin = File.ReadAllText(fileDirectory, Encoding.UTF8).Trim();
                                if (newPin.Length > 0 && newPin.Length < 7)
                                {
                                    _pin = newPin;
                                    LyokoLogger.Log(Name, "User pin successfully loaded!");
                                    checkVersion();
                                    _token = FetchToken();
                                    Listener.StartListening();
                                }
                                else
                                {
                                    LyokoLogger.Log(Name, "The PIN is empty!");
                                }

                                File.Delete(batFileName);
                            }
                            else
                            {
                                LyokoLogger.Log(Name,
                                    @"Please input the pin given by ReplikAPP in \ReplikAPP\UserPin.txt");
                                LyokoLogger.Log(Name,
                                    "The plugin will disable now, restart the game once the pin is in the file.");
                            }
                        }
                        catch (Exception exception)
                        {
                            LyokoLogger.Log(Name,
                                $"Something went wrong when reading the pin! Please check that only your pin is inside UserPin.txt:\n {exception.ToString()}");
                        }
                    };
                    return true;
                }
                catch (Exception e)
                {
                    LyokoLogger.Log(Name,
                        $"Something went wrong when reading the pin! Please check that only your pin is inside UserPin.txt:\n {e.ToString()}");
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
                    $"Something went wrong creating the config directory:\n {e.ToString()}, check if you have write access to the directory");
                return false;
            }

            LyokoLogger.Log(Name,
                @"Please input the pin given by ReplikAPP in UserPin.txt (in ReplikAPP's folder)");
            return true;
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
                        
                        localization.Add(cleanString(key), cleanString((string) dictionary[key]));
                    }
                }
            }
            catch (Exception e)
            {
                LyokoLogger.Log("ReplikAPP",e.ToString());
            }
        }

        private string cleanString(string check)
        {
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
            WebClient wc = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => true;
            wc.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
            {
                if(e.Error == null && !e.Cancelled)
                {
                    if (e.Result != "\"" + Version + "\"")
                    {
                        FireBasePush.SendMessage(localize("versionCheck.title"), localize("versionCheck.subtitle"));
                        LyokoLogger.Log(Name,"The plugin isn't updated!");
                    }
                    else
                    {
                        LyokoLogger.Log(Name,"The plugin is updated!");   
                    }
                }
                else
                {
                    LyokoLogger.Log(Name,"An error has occured while checking the version!\n" + e.Error.Message + "\n" + e.Error.StackTrace);
                }
            };
            string dataKey = ServerKey.GetDataKey();
            wc.DownloadStringAsync(new Uri("https://lyokoapp.firebaseio.com/-VERSION.json?auth=" + dataKey));
        }
    }
}