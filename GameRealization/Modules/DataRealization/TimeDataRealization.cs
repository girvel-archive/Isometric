using System;
using GameCore.Modules.TimeModule;

namespace GameRealization.Modules.DataRealization
{
    internal static class TimeDataRealization
    {
        internal static void Init()
        {
            GameDate.Data = new TimeData() 
            {
                DaysInMonth = 60,
                MonthsInYear = 6,
                DaysInWeek = 6,
            };

            GameDate.Data.RefreshDependentValues();
        }
    }
}

