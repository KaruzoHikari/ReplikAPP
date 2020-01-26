using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using LyokoAPI.Events;
using LyokoAPI.Plugin;

namespace ReplikAPP_Plugin
{
    public class Main : LyokoAPIPlugin
    {
        public override string Name { get; } = "ReplikAPP";
        public override string Author { get; } = "KaruzoHikari and Appryl";
        private static string _token = "";

        private static Dictionary<string,string> localization = new Dictionary<string, string>();

        protected override bool OnEnable()
        {
            if (!ReadToken())
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

        private bool ReadToken()
        {
            if (!CheckLegacyToken())
            {
                string directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + @"\ReplikAPP";
                string fileDirectory = directory + @"\UserToken.txt";
                if (Directory.Exists(directory) && File.Exists(fileDirectory))
                {
                    try
                    {
                        string token = File.ReadAllText(fileDirectory, Encoding.UTF8);
                        if (token.Length > 0)
                        {
                            _token = token;
                            LyokoLogger.Log(Name, "User token successfully loaded!");
                            return true;
                        }

                        LyokoLogger.Log(Name, @"Please input the token given by ReplikAPP in \ReplikAPP\UserToken.txt");
                        LyokoLogger.Log(Name, "The plugin will disable now, restart the game once the token is in the file.");
                        return false;
                    }
                    catch (Exception e)
                    {
                        LyokoLogger.Log(Name,
                            "Something went wrong when reading the token! Please check that only your token is inside UserToken.txt");
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
                    @"Please input the token given by ReplikAPP in UserToken.txt (in ReplikAPP's folder)");
                LyokoLogger.Log(Name, "The plugin will disable now, restart the game once the token is in the file.");
                return false;
            }
            return true;
        }

        private bool CheckLegacyToken()
        {
            string directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + @"\LyokoAPP";
            string fileDirectory = directory + @"\UserToken.txt";
            if (Directory.Exists(directory) && File.Exists(fileDirectory))
            {
                try
                {
                    string newDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + @"\ReplikAPP";
                    string newFileDirectory = newDirectory + @"\UserToken.txt";

                    Directory.CreateDirectory(newDirectory);
                    File.Move(fileDirectory, newFileDirectory);
                    Directory.Delete(directory, true);
                    LyokoLogger.Log(Name, "The token was successfully ported over from the LyokoAPP folder!");
                    return true;
                }
                catch (Exception e)
                {
                    LyokoLogger.Log(Name, $"Something went wrong moving the token from LyokoAPP to ReplikAPP's folder. Please remove the LyokoAPP folder manually.': {e.ToString()}, check if you have write access to the directory");
                }
            }

            return false;
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
    }
}
