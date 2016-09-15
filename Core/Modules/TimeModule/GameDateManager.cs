using Isometric.Core.Modules.TickModule;

namespace Isometric.Core.Modules.TimeModule
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

