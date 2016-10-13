using System;
using Isometric.Core.Modules.SettingsModule;

namespace Isometric.Core.Modules.TimeModule
{
    // TODO 1.1 GameDate to common
    [Serializable]
    public struct GameDate
    {
        public readonly int TotalDay;



        public int Year => (int)Math.Floor((double)TotalDay / DaysInYear);

        public int TotalMonth => (int)Math.Floor((double)TotalDay / DaysInMonth);

        public int Month => (int)TotalMonth % MonthsInYear;

        public int TotalWeek => (int)Math.Floor((double)TotalDay / DaysInWeek);

        public int Week => TotalWeek % (int)Math.Floor(WeeksInYear);

        public int Day => TotalDay % DaysInMonth;

        public GameSeason Season => (GameSeason)Math.Floor((double)TotalDay / DaysInSeason);



        private const string
            DayFormatSpecifier = "d",
            WeekFormatSpecifier = "w",
            MonthFormatSpecifier = "m",
            YearFormatSpecifier = "y",
            TotalDayFormatSpecifier = "D",
            TotalWeekFormatSpecifier = "W",
            TotalMonthFormatSpecifier = "M";


        [GameConstant]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public static byte DaysInMonth { get; private set; }

        [GameConstant]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public static byte DaysInWeek { get; private set; }

        [GameConstant]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public static byte MonthsInYear { get; private set; }
        

        public static short DaysInSeason => (short)(DaysInYear / typeof(GameSeason).GetEnumValues().Length);

        public static short DaysInYear => (short)(DaysInMonth * MonthsInYear);

        public static double WeeksInYear => (double)DaysInYear / DaysInWeek;



        public GameDate(int totalDay)
        {
            TotalDay = totalDay;
        }

        public GameDate(int years, int months, int days)
        {
            TotalDay = DaysInYear * years + DaysInYear * months + days;
        }



        public static GameDate operator +(GameDate d1, GameDate d2)
        {
            return new GameDate(d1.TotalDay + d2.TotalDay);
        }

        public static GameDate operator -(GameDate d1, GameDate d2)
        {
            return new GameDate(d1.TotalDay - d2.TotalDay);
        }

        public static GameDate operator +(GameDate d, int days)
        {
            return new GameDate(d.TotalDay + days);
        }

        public static GameDate operator -(GameDate d, int days)
        {
            return new GameDate(d.TotalDay - days);
        }

        public static bool operator >(GameDate d1, GameDate d2)
        {
            return d1.TotalDay > d2.TotalDay;
        }

        public static bool operator <(GameDate d1, GameDate d2)
        {
            return d1.TotalDay < d2.TotalDay;
        }



        public override string ToString() => $"{typeof (GameDate).Name}; {Year}.{Month}.{TotalDay} ({Week})";

        public string ToString(string format)
        {
            return format
                .Replace(DayFormatSpecifier, Day.ToString())
                .Replace(WeekFormatSpecifier, Week.ToString())
                .Replace(MonthFormatSpecifier, Month.ToString())
                .Replace(YearFormatSpecifier, Year.ToString())
                .Replace(TotalDayFormatSpecifier, TotalDay.ToString())
                .Replace(TotalWeekFormatSpecifier, TotalWeek.ToString())
                .Replace(TotalMonthFormatSpecifier, TotalMonth.ToString());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GameDate))
            {
                return base.Equals(obj);
            }

            return TotalDay == ((GameDate) obj).TotalDay;
        }

        public override int GetHashCode()
        {
            return TotalDay.GetHashCode();
        }
    }
}

