using System;
using IsometricCore.Modules.TickModule;

namespace IsometricCore.Modules.TimeModule
{
    public class GameDateManager : IIndependentChanging
    {
        public static GameDateManager Instance { get; set; }



        public GameDate Current;



        void IIndependentChanging.Tick()
        {
            Current.TotalDays += ClocksManager.Data.DaysInTick;
        }
    }
}

