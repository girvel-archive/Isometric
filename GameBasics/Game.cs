using System;
using System.Collections.Generic;
using System.Threading;
using GameBasics.Buildings;
using GameBasics.Structures;
using VisualClient;

namespace GameBasics
{
    [Serializable]
    public class Game : IRefreshable
    {
        public World World { get; set; }
        public Graph<BuildingPattern> BuildingGraph { get; set; } 
        public List<Player> Players { get; set; }

        public int Seed { get; }



        protected Game(int seed)
        {
            Seed = seed;
            Players = new List<Player>();
        }

        public Game()
        {
        }

        public void Refresh()
        {
            World.Refresh();
            Players.ForEach(p => p.Refresh());
        }

        public void RefreshLoop()
        {
            while (true)
            {
                Refresh();
                Thread.Sleep(RefreshHelper.RefreshPeriodMilliseconds);
            }
        }

        public void CheckVersion(ProgramVersion version)
        {
            
        }
    }
}
