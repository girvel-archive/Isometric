using System;

namespace Isometric.Core.Modules.TimeModule
{
    // TODO 1.1 GameDate to common
    [Serializable]
    public struct GameDate
    {
        #region Data singleton

        [Obsolete("using backing field")]
        private static TimeData _data;

        #pragma warning disable 0618

        public static TimeData Data {
            get { return _data ?? (_data = new TimeData()); }

            set {
                #if DEBUG
                if (_data != null)
                {
                    throw new ArgumentException("Data is already set");
                }
                #endif

                _data = value;
            }
        }

        #pragma warning restore 0618

        #endregion



        public int TotalDay;



        public int Year => (int)Math.Floor((double)TotalDay / Data.DaysInYear);

        public int TotalMonth => (int)Math.Floor((double)TotalDay / Data.DaysInMonth);

        public int Month => (int)TotalMonth % Data.MonthsInYear;

        public int TotalWeek => (int)Math.Floor((double)TotalDay / Data.DaysInWeek);

        public int Week => TotalWeek % (int)Math.Floor(Data.WeeksInYear);

        public int Day => TotalDay % Data.DaysInMonth;

        public GameSeason Season => (GameSeason)Math.Floor((double)TotalDay / Data.DaysInSeason);



        private const string
            DayFormatSpecifier = "d",
            WeekFormatSpecifier = "w",
            MonthFormatSpecifier = "m",
            YearFormatSpecifier = "y",
            TotalDayFormatSpecifier = "D",
            TotalWeekFormatSpecifier = "W",
            TotalMonthFormatSpecifier = "M";

        public GameDate(int totalDay)
        {
            TotalDay = totalDay;
        }

        public GameDate(int years, int months, int days)
        {
            TotalDay = Data.DaysInYear * years + Data.DaysInYear * months + days;
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
    }
}

