using System;
using IsometricCore.Modules.TimeModule;

namespace IsometricImplementation.Modules.DataRealization
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

