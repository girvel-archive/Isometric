using System;
using GameCore.Modules.TimeModule;

namespace GameCore.Modules
{
	[Serializable]
	public class SingleRandom
	{
		#region Singleton-part

		private static Random _instance;
		public static Random Instance {
			get {
				if (_instance == null)
				{
					_instance = CustomSeed ? new Random(Seed) : new Random();
				}

				return _instance;
			}
		}

		private SingleRandom() {}

		#endregion



		private static int _seed;
		public static int Seed {
			get {
				return _seed;
			}
			set {
#if DEBUG
				if (_instance != null)
				{
					throw new ArgumentException(
						"SingleRandom.Instance is already set; seed can not be changed");
				}
#endif

				_seed = value;
				CustomSeed = true;
			}
		}

		public static bool CustomSeed { get; set; }



		public static GameDate Next(GameDate min, GameDate max)
		{
			return new GameDate(
				Instance.Next(
					min.TotalDays,
					max.TotalDays));
		}

	}
}

