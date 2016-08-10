using System;
using GameCore.Modules.TickModule;

namespace GameRealization.Modules.DataRealization
{
    public static class TickDataRealization
	{
        static TickDataRealization ()
		{
            TickData.Instance = new TickData() 
            {
                DaysInTick = 60,
            };

            TickData.Instance.RefreshDependentValues();
		}
	}
}

