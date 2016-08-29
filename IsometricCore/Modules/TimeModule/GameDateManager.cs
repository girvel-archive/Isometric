using System;
using IsometricCore.Modules.TickModule;

namespace IsometricCore.Modules.TimeModule
{
    public class GameDateManager : IIndependentChanging
    {
        #region Singleton-part

        private static GameDateManager _instance;
        public static GameDateManager Instance {
            get { return _instance ?? (_instance = new GameDateManager()); }

            set {
                if (_instance != null)
                {
                    throw new ArgumentException("GameDateManager.Instance is already set");
                }

                _instance = value;
            }
        }

        private GameDateManager() {}

        #endregion



        public GameDate Current;



        void IIndependentChanging.Tick()
        {
            Current.TotalDays += ClocksManager.Data.DaysInTick;
        }
    }
}

