using IsometricCore.Modules.TimeModule;

namespace IsometricImplementation.Modules.DataImplementation
{
    internal static class TimeDataImplementation
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

