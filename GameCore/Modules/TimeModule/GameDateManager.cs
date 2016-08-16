using System;
using GameCore.Modules.TickModule;
using GameCore.Modules.TimeModule;

namespace GameCore.Modules.TimeModule
{
	public class GameDateManager : IIndependentChanging
	{
		#region Singleton-part

		private static GameDateManager _instance;
		public static GameDateManager Instance {
			get {
				if (_instance == null)
				{
					_instance = new GameDateManager();
				}

				return _instance;
			}

			set {
				if (_instance != null)
				{
					throw new ArgumentException("GameDateManager.Instance is already set");
				}

				_instance = value;
			}
		}

		private GameDateManager() {}

		#endregion



		public static GameDate CurrentDate => Instance.Current;

        private GameDate Current;



		void IIndependentChanging.Tick()
		{
            Current.TotalDays += ClocksManager.Data.DaysInTick;
		}
	}
}

