using System;

namespace GameCore.Modules.TimeModule
{
	[Serializable]
	public struct GameDate
	{
		public long TotalDays;

		public int Year => Math.Floor((double)TotalDay / GlobalData.Instance.DaysInYear);

		public int TotalMonth => Math.Floor((double)TotalDay / GlobalData.Instance.DaysInMonth);

		public int Month => TotalMonth % GlobalData.Instance.MonthsInYear;

		public int TotalWeek => Math.Floor((double)TotalDay / GlobalData.Instance.DaysInWeek);

		public int Week => TotalWeek % GlobalData.Instance.WeeksInYear;

		public GameSeason Season => Math.Floor((double)TotalDay / GlobalData.Instance.DaysInSeason);



		public GameDate(int totalDays)
		{
			TotalDays = totalDays;
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

