using System;
using System.Collections.Generic;
using Isometric.Core.Modules.TickModule;

namespace Isometric.Core.Modules.PlayerModule
{
    public class PlayersManager : IIndependentChanging
    {
        public List<Player> Players { get; set; }



        public PlayersManager()
        {
            Players = new List<Player>();
        }



        public void Tick()
        {
            foreach (var player in Players)
            {
                player.Tick();
            }
        }
    }
}

