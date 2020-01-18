using LyokoAPI.Events;
using LyokoAPI.VirtualStructures;
using LyokoAPI.VirtualStructures.Interfaces;

namespace LyokoAPP_Plugin
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
            string title = "A tower has been activated!";
            string body = "Tower nº" + tower.Number + " in the " + Main.GetUppercaseNames(tower.Sector.Name) + "!";
            FireBasePush.SendMessage(title, body);
            //DataPush.SendData(tower);
        }

        private static void OnTowerDeactivation(ITower tower)
        {
            string title = "Good job! Tower deactivated!";
            string body = "Tower nº" + tower.Number + " in the " + Main.GetUppercaseNames(tower.Sector.Name) + ".";
            FireBasePush.SendMessage(title, body);
        }

        private static void OnTowerHijack(ITower tower, APIActivator oldActivator, APIActivator newActivator)
        {
            string title = "Huh? A tower has been hijacked!";
            string body = "Tower nº" + tower.Number + " in the " + Main.GetUppercaseNames(tower.Sector.Name) + ", from " +
                          Main.GetUppercaseNames(oldActivator.ToString()) + " to " + Main.GetUppercaseNames(newActivator.ToString()) + "!";
            FireBasePush.SendMessage(title, body);
        }
    }
}
