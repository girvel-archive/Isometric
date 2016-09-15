namespace Isometric.Core.Modules.TimeModule
{
    public class TimeData
    {
        public TimeData() {}



        public byte DaysInMonth;
        public byte DaysInWeek;
        public byte MonthsInYear;

        public short DaysInSeason { get; private set; }

        public short DaysInYear { get; private set; }
        public double WeeksInYear { get; private set; }



        public void RefreshDependentValues()
        {
            DaysInYear = (short)(DaysInMonth * MonthsInYear);
            WeeksInYear = DaysInYear / DaysInWeek;

            DaysInSeason = (short)(DaysInYear / typeof(GameSeason).GetEnumValues().Length);
        }
    }
}

