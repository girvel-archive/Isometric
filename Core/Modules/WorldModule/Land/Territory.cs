using System;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using VectorNet;

namespace Isometric.Core.Modules.WorldModule.Land
{
    [Serializable]
    public class Territory : IIndependentChanging
    {
        public TerritoryPattern Pattern { get; set; }

        public Building[,] BuildingGrid { get; }

        public Building this[IntVector position] 
        {
            get
            {
                return BuildingGrid[position.X, position.Y];
            }
            set
            {
                BuildingGrid[position.X, position.Y] = value;
            }
        }



        public int Seed { get; }

        public TerritoryGenerationType Type { get; set; } = TerritoryGenerationType.Wild;

        protected Random Random;



        public Territory(TerritoryPattern pattern, int seed) 
        {
            Pattern = pattern;
            Seed = seed;

            Random = new Random(Seed);

            BuildingGrid = new Building[World.Data.TerritorySize, World.Data.TerritorySize];

            Pattern.Generate(this, seed);
        }



        #region Interface

        void IIndependentChanging.Tick()
        {
            foreach (IIndependentChanging building in BuildingGrid)
            {
                building.Tick();
            }

            Pattern.Tick?.Invoke(this);
        }

        #endregion
    }
}

