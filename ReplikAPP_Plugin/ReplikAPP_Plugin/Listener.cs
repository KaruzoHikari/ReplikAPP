using System;
using LyokoAPI.Events;
using LyokoAPI.VirtualStructures;
using LyokoAPI.VirtualStructures.Interfaces;

namespace ReplikAPP_Plugin
{
    public class Listener
    {
        private static bool _listening;

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
    }
}
