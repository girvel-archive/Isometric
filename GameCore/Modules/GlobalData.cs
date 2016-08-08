using System;
using GameCore.Modules.PlayerModule;
using GameCore.Modules.TimeModule;

namespace GameCore.Modules
{
	[Serializable]
	public class GlobalData
	{
		#region Singleton-part

		private static GlobalData _instance;
		public static GlobalData Instance {
			get {
				if (_instance == null)
				{
					_instance = new GlobalData();
				}

				return _instance;
			}
			set {
				if (_instance == null)
				{
					_instance = value;
				}
				else
				{
					throw new ArgumentException("GlobalData.Instance is already set");
				}
			}
		}

        #endregion



		public Resources DefaultPlayerResources;

		public short TerritorySize;



		// FIXME Отделить реализацию GlobalData
		public byte DaysInMonth = 60;
		public byte DaysInWeek = 6;
		public byte MonthsInYear = 6;

		public short DaysInSeason = 90;

		public short DaysInYear = DaysInMonth * MonthsInYear;
		public double WeeksInYear = DaysInYear / DaysInWeek;

		public short DaysInTick = 60;



		public GameDate 
			MinimalNewLeaderAge,
			MaximalNewLeaderAge,
			MinimalLeaderLifeDuration,
			MaximalLeaderLifeDuration;
	}
}

