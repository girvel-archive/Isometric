using System;
using GameCore.Modules.TickModule;

namespace GameRealization.Modules.DataRealization
{
    public static class TickDataRealization
    {
        internal static void Init()
        {
            ClocksManager.Data = new TickData() 
                {
                    DaysInTick = 60,
                    TickLengthMilliseconds = 60000,
                };

            ClocksManager.Data.RefreshDependentValues();
        }
    }
}

