using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace ShortCommandsV2
{
    public class SCPlayer
    {
        public int Index { get; set; }
        public TSPlayer TSPlayer { get { return TShock.Players[Index]; } }
        public DateTime lastSB;
        public DateTime lastSM;
        public DateTime lastBN;
        public bool hasShine;
        public bool hasSkin;
        public bool hasGills;
        public bool hasInvis;
        public bool hasWaterW;
        public bool hasGrav;
        public bool hasLife;
        public bool hasFall;
        public bool hasOrb;
        public bool hasRedFairy;
        public bool hasGreenFairy;
        public bool hasBlueFairy;
        public bool needsUpdating;
        public DateTime lastBuffUpdate;
        public Timer atimer;
        public bool canSB;

        public SCPlayer(int index)
        {
            Index = index;
            lastSB = DateTime.Now;
            lastSM = DateTime.Now;
            lastBN = DateTime.Now;
            needsUpdating = true;
            hasShine = false;
            hasSkin = false;
            hasGills = false;
            hasInvis = false;
            hasWaterW = false;
            hasGrav = false;
            hasLife = false;
            hasFall = false;
            hasOrb = false;
            hasRedFairy = false;
            hasGreenFairy = false;
            hasBlueFairy = false;
/*            atimer = new Timer(1000);

            atimer.Elapsed += new ElapsedEventHandler(timer);

            public void timer(object sender, ElapsedEventArgs args)
            {
                foreach (SCPlayer Player in SCommands.Players)
                {
                }
            }*/
        }
    }
}
