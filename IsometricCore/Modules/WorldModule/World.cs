﻿using System;
using IsometricCore.Modules.PlayerModule;
using VectorNet;
using IsometricCore.Modules.WorldModule.Land;
using IsometricCore.Modules.TickModule;

namespace IsometricCore.Modules.WorldModule
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
            get { return _data ?? (_data = new WorldData()); }

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



        // FIXME test it
        protected int SeedForPosition(int x, int y)
        {
            return (int)((decimal)Seed * (x * Data.TerritorySize + y) / (decimal)Math.Pow(Data.TerritorySize, 2));
        }
    }
}

