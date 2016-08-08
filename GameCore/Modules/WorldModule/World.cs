using System;
using GameCore.Structures;
using GameCore.Modules.WorldModule;
using GameCore.Modules.PlayerModule;
using VectorNet;

namespace GameCore.Modules.WorldModule
{
	[Serializable]
	public class World
	{
		#region Singleton-part

		private static World _instance;
		public static World Instance {
			get {
				if (_instance == null)
				{
					_instance = new World();
				}

				return _instance;
			}
		}

		private World() 
		{
			LandGrid = new LazyArray<Territory>(
				GlobalData.Instance.TerritorySize,
				GlobalData.Instance.TerritorySize,
				_generateTerritory);
		}

		#endregion



		public LazyArray<Territory> LandGrid { get; }



		public static Func<Player, Territory> NewPlayerTerritory { get; set; }

		private static Func<IntVector, Territory> _generateTerritory { get; set; }
	}
}

