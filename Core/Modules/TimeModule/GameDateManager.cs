using Isometric.Core.Modules.TickModule;

namespace Isometric.Core.Modules.TimeModule
{
    public class GameDateManager : IIndependentChanging
    {
        public GameDate Current;



        void IIndependentChanging.Tick()
        {
            Current += ClocksManager.DaysInTick;
        }
    }
}

