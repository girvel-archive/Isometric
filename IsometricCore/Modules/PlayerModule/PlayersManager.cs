using System;
using System.Collections.Generic;
using IsometricCore.Modules.TickModule;

namespace IsometricCore.Modules.PlayerModule
{
    public class PlayersManager : IIndependentChanging
    {
        #region Singleton-part

        [Obsolete("using backing field")]
        private static PlayersManager _instance;

        #pragma warning disable 618

        public static PlayersManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayersManager();
                }

                return _instance;
            }

            set
            {
                #if DEBUG

                if (_instance != null)
                {
                    throw new ArgumentException("Instance is already set");
                }

                #endif

                _instance = value;
            }
        }

        #pragma warning restore 618

        #endregion



        public List<Player> Players { get; set; }



        private PlayersManager()
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

