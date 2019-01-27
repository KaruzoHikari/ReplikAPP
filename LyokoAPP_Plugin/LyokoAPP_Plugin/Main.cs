using System;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Xml;
using LyokoAPI.Events;
using LyokoAPI.Plugin;

namespace LyokoAPP_Plugin
{
    public class Main : LyokoAPIPlugin
    {
        public override string Name { get; } = "LyokoAPP_Plugin";
        public override string Author { get; } = "KaruzoHikari";
        private static string _token = "";
        
        protected override bool OnEnable()
        {
            if (!ReadToken())
            {
                return false;
            }
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
            string directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + @"\LyokoAPP";
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

                    LyokoLogger.Log(Name, @"Please input the token given by LyokoAPP in \LyokoAPP\UserToken.txt");
                    return false;
                }
                catch (Exception e)
                {
                    LyokoLogger.Log(Name,
                        "Something went wrong when reading the token! Please check that only your token is inside UserToken.txt");
                    return false;
                }
            }

            Directory.CreateDirectory(directory);
            File.Create(fileDirectory);
            LyokoLogger.Log(Name, @"Please input the token given by LyokoAPP in UserToken.txt (in LyokoAPP's folder)");
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

            return char.ToUpperInvariant(name[0]) + name.Substring(1).ToLowerInvariant();
        }
    }
}