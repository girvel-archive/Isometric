using System;

namespace IsometricCore.Modules.TimeModule
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



        public int TotalDays;

        public int Year => (int)Math.Floor((double)TotalDays / Data.DaysInYear);

        public int TotalMonth => (int)Math.Floor((double)TotalDays / Data.DaysInMonth);

        public int Month => (int)TotalMonth % Data.MonthsInYear;

        public int TotalWeek => (int)Math.Floor((double)TotalDays / Data.DaysInWeek);

        public int Week => TotalWeek % (int)Math.Floor(Data.WeeksInYear);

        public GameSeason Season => (GameSeason)Math.Floor((double)TotalDays / Data.DaysInSeason);



        public GameDate(int totalDays)
        {
            TotalDays = totalDays;
        }

        public GameDate(int years, int months, int days)
        {
            TotalDays = Data.DaysInYear * years + Data.DaysInYear * months + days;
        }

        public static GameDate operator +(GameDate d1, GameDate d2)
        {
            return new GameDate(d1.TotalDays + d2.TotalDays);
        }

        public static GameDate operator -(GameDate d1, GameDate d2)
        {
            return new GameDate(d1.TotalDays - d2.TotalDays);
        }

        public static GameDate operator +(GameDate d, int days)
        {
            return new GameDate(d.TotalDays + days);
        }

        public static GameDate operator -(GameDate d, int days)
        {
            return new GameDate(d.TotalDays - days);
        }

        public static bool operator >(GameDate d1, GameDate d2)
        {
            return d1.TotalDays > d2.TotalDays;
        }

        public static bool operator <(GameDate d1, GameDate d2)
        {
            return d1.TotalDays < d2.TotalDays;
        }
    }
}

