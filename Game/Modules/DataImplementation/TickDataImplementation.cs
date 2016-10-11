using Isometric.Core.Modules.TickModule;

namespace Isometric.Game.Modules.DataImplementation
{
    public static class TickDataImplementation
    {
        internal static void Init()
        {
            ClocksManager.Data = new TickData
            {
                DaysInTick = 60,
                TickLengthMilliseconds = 1000,
            };

            ClocksManager.Data.RefreshDependentValues();
        }
    }
}

