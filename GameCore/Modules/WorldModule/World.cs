using System;
using GameCore.Structures;
using GameCore.Modules.WorldModule;
using GameCore.Modules.PlayerModule;
using VectorNet;
using GameCore.Modules.WorldModule.Land;

namespace GameCore.Modules.WorldModule
{
	[Serializable]
	public class World
	{
        #region Data singleton

        [Obsolete("using backing field")]
        private static WorldData _data;

        #pragma warning disable 0618

        public static WorldData Data {
            get {
                if (_data == null)
                {
                    _data = new WorldData();
                }

                return _data;
            }

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
				Data.TerritorySize,
				Data.TerritorySize,
				_generateTerritory);
		}

		#endregion



		public LazyArray<Territory> LandGrid { get; }



		public Func<Player, Territory> NewPlayerTerritory { get; set; }

		private Func<IntVector, Territory> _generateTerritory { get; set; }
	}
}

