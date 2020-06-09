using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ManelePlayer
    {
        public static System.Media.SoundPlayer player;
        private static readonly string[] manelari = { "Guta", "Piticu", "Salam", "Minune" };
        private static readonly string stop = "StopManea";

        public static void RecieveMessage(string message)
        {
            if (message.Contains(stop))
                StopManea();


            foreach (string manelar in manelari)
                if (message.Contains(manelar))
                {
                    PlayManea(manelar);
                    return;
                }
        }

        public static void StopManea()
        {
            player?.Stop();
        }

        public static void PlayManea(string manelar)
        {
            player?.Stop();
            player = new System.Media.SoundPlayer($"../../{manelar}.wav");
            player?.Play();
        }
    }
}
