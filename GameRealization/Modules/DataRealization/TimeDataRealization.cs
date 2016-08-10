using System;
using GameCore.Modules.TimeModule;

namespace GameRealization.Modules.DataRealization
{
    internal static class TimeDataRealization
	{
        static TimeDataRealization ()
		{
            TimeData.Instance = new TimeData() 
            {
                DaysInMonth = 60,
                MonthsInYear = 6,
                DaysInWeek = 6,
                DaysInSeason = 90,
            };

            TimeData.Instance.RefreshDependentValues();
		}
	}
}

