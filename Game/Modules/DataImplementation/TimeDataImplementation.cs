using Isometric.Core.Modules.TimeModule;

namespace Isometric.Game.Modules.DataImplementation
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

