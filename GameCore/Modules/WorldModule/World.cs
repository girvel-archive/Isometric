using System;
using System.Linq;
using GameCore.Structures;
using GameCore.Modules.WorldModule;
using GameCore.Modules.PlayerModule;
using VectorNet;
using GameCore.Modules.WorldModule.Land;
using GameCore.Modules.TickModule;

namespace GameCore.Modules.WorldModule
{
	[Serializable]
	public class World : IIndependentChanging
	{
        #region Data singleton

        [Obsolete("using backing field")]
        [NonSerialized]
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

        [NonSerialized]
		private static World _instance;
		public static World Instance 
        {
			get 
            {
				if (_instance == null)
				{
					_instance = new World();
				}

				return _instance;
			}

            set
            {
                #if DEBUG

                if (_instance != null)
                {
                    throw new ArgumentException("Instance is already set");
                }

                #endif

                _instance = value;
            }
		}

        #endregion



        public World() 
        {
            LandGrid = new Territory[Data.TerritorySize, Data.TerritorySize];
        }



		public Territory[,] LandGrid { get; }



        public void CheckVersion()
        {

        }

        public Territory LazyGetTerritory(IntVector position) => LazyGetTerritory(position.X, position.Y);

        public Territory LazyGetTerritory(int x, int y)
        {
            if (LandGrid[x, y] == null)
            {
                LandGrid[x, y] = Data.GenerateTerritory(LandGrid, x, y);
            }

            return LandGrid[x, y];
        }

        public Territory NewPlayerTerritory(Player player)
        {
            Territory result;
            do
            {
                result = LazyGetTerritory(SingleRandom.Next(Data.TerritoryVectorSize));
            }
            while (result.Type != TerritoryGenerationType.Wild);

            Data.NewPlayerTerritory(player, result);
            result.Type = TerritoryGenerationType.Wild;

            return result;
        }



        public void Tick()
        {
            foreach (IIndependentChanging territory in LandGrid)
            {
                if (territory == null)
                {
                    continue;
                }

                territory.Tick();
            }
        }
	}
}

