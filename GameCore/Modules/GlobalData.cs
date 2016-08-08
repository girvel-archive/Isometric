using System;
using GameCore.Modules.PlayerModule;
using GameCore.Modules.TimeModule;
using VectorNet;
using GameCore.Extensions;

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
                    var ex = new ArgumentException("GlobalData.Instance is already set");
#if DEBUG
					throw ex;
#else
                    OnUnknownException(_instance, new DelegateExtensions.ExceptionEventArgs(ex));
#endif
				}
			}
		}

        #endregion



		public Resources DefaultPlayerResources;

		public short TerritorySize;
        public IntVector TerritoryVectorSize { get; private set; }



		public byte DaysInMonth;
		public byte DaysInWeek;
		public byte MonthsInYear;

        public short DaysInSeason { get; private set; }

        public short DaysInYear { get; private set; }
        public double WeeksInYear { get; private set; }

		public short DaysInTick;



		public GameDate 
			MinimalNewLeaderAge,
			MaximalNewLeaderAge,
			MinimalLeaderLifeDuration,
			MaximalLeaderLifeDuration;



        public event EventHandler<DelegateExtensions.ExceptionEventArgs> OnUnknownException;



        public void RefreshValues()
        {
            TerritoryVectorSize = new IntVector(TerritorySize, TerritorySize);

            DaysInYear = DaysInMonth * MonthsInYear;
            WeeksInYear = DaysInYear / DaysInWeek;

            DaysInSeason = DaysInYear / typeof(GameSeason).GetEnumValues().Length;
        }
	}
}

