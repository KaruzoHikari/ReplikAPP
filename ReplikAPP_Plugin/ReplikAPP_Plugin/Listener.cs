using System;
using LyokoAPI.Events;
using LyokoAPI.VirtualStructures;
using LyokoAPI.VirtualStructures.Interfaces;

namespace ReplikAPP_Plugin
{
    public class Listener
    {
        private static bool _listening;
        private static string _lan = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;

        public static void StartListening()
        {
            if (_listening) { return; }

            TowerActivationEvent.Subscribe(OnTowerActivation);
            TowerDeactivationEvent.Subscribe(OnTowerDeactivation);
            TowerHijackEvent.Subscribe(OnTowerHijack);

            _listening = true;
        }

        public static void StopListening()
        {
            if (!_listening) { return; }

            TowerActivationEvent.Unsubscribe(OnTowerActivation);
            TowerDeactivationEvent.Unsubscribe(OnTowerDeactivation);
            TowerHijackEvent.Unsubscribe(OnTowerHijack);

            _listening = false;
        }

        private static void OnTowerActivation(ITower tower)
        {
            string title = Main.localize("tower.activation.title");
            string body = String.Format(Main.localize("tower.activation.subtitle"),tower.Number,Main.localize("sector." + tower.Sector.Name.ToLowerInvariant()));
            FireBasePush.SendMessage(title, body);
            DataPush.SendData(tower);
        }

        private static void OnTowerDeactivation(ITower tower)
        {
            string title = Main.localize("tower.deactivation.title");
            string body = String.Format(Main.localize("tower.deactivation.subtitle"),tower.Number,Main.localize("sector." + tower.Sector.Name.ToLowerInvariant()));
            FireBasePush.SendMessage(title, body);
        }

        private static void OnTowerHijack(ITower tower, APIActivator oldActivator, APIActivator newActivator)
        {
            string title = Main.localize("tower.hijack.title");
            string body = String.Format(Main.localize("tower.hijack.subtitle"),tower.Number,Main.localize("sector." + tower.Sector.Name.ToLowerInvariant()),
                Main.GetUppercaseNames(oldActivator.ToString()),Main.GetUppercaseNames(newActivator.ToString()));
            FireBasePush.SendMessage(title, body);
        }

        private static string GenMessage(ITower tower, APIActivator oldActivator, APIActivator newActivator, int status)
        {
            string title;
            string body;
            
            if (_lan == "FR")
            {
                body = "Tour nº" + tower.Number + " dans le territoire " + Main.GetUppercaseNames(tower.Sector.Name) + ".";
                if (status == 0)
                {
                    title = "Une tour a été activée !";
                } else if (status == 1)
                {
                    title = "Bien joué ! La tour a été desactivée !";
                } else if (status == 3)
                {
                    title = "Quoi ? Une tour a été piratée !";
                    body = "Tour nº" + tower.Number + " dans le territoire " + Main.GetUppercaseNames(tower.Sector.Name) + ", de " +
                           Main.GetUppercaseNames(oldActivator.ToString()) + " to " + Main.GetUppercaseNames(newActivator.ToString()) + "!";
                }
                body = "Tour nº" + tower.Number + " dans le territoire " + Main.GetUppercaseNames(tower.Sector.Name) + ".";
            } else if (_lan == "ES")
            {
                
            } else
            {
                
            }
            FireBasePush.SendMessage(title, body);
        };
    }
}
