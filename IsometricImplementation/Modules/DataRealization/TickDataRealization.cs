using IsometricCore.Modules.TickModule;

namespace IsometricImplementation.Modules.DataRealization
{
    public static class TickDataRealization
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

