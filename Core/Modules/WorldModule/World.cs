using System;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule.Land;
using VectorNet;

namespace Isometric.Core.Modules.WorldModule
{
    [Serializable]
    public class World : IIndependentChanging
    {
        [NonSerialized]
        public static WorldData Data;



        #region Singleton-part

        [NonSerialized]
        private static World _instance;
        public static World Instance 
        {
            get { return _instance ?? (_instance = new World(SingleRandom.Instance.Next())); }

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



        public World(int seed) 
        {
            LandGrid = new Territory[Data.TerritorySize, Data.TerritorySize];

            Seed = seed;
            Random = new Random(Seed);
        }



        public Territory[,] LandGrid { get; }

        public int Seed { get; }

        protected Random Random;



        public void CheckVersion()
        {

        }

        public Territory LazyGetTerritory(IntVector position) => LazyGetTerritory(position.X, position.Y);

        public Territory LazyGetTerritory(int x, int y)
        {
            if (LandGrid[x, y] == null)
            {
                LandGrid[x, y] = Data.GenerateTerritory(LandGrid, x, y, SeedForPosition(x, y));
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



        // TODO F test it
        protected int SeedForPosition(int x, int y)
        {
            return (int)((decimal)Seed * (x * Data.TerritorySize + y) / (decimal)Math.Pow(Data.TerritorySize, 2));
        }
    }
}

