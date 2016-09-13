using IsometricCore.Modules.TickModule;

namespace IsometricImplementation.Modules.DataImplementation
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

