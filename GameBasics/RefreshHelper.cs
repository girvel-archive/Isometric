namespace GameBasics
{
    public static class RefreshHelper
    {
        public const int RefreshPeriodMilliseconds = 1000 * 60 * 2;
        public const int RefreshPeriodDays = 1;
        public static int DaysInYear => World.Date.DaysInMonth * World.Date.MonthsInYear;
    }
}

