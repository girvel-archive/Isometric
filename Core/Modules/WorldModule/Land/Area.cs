using System;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using VectorNet;

namespace Isometric.Core.Modules.WorldModule.Land
{
    [Serializable]
    public class Area : IIndependentChanging
    {
        public AreaPattern Pattern { get; set; }

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

        public AreaGenerationType Type { get; set; } = AreaGenerationType.Wild;

        protected Random Random;



        public Area(AreaPattern pattern, int seed) 
        {
            Pattern = pattern;
            Seed = seed;

            Random = new Random(Seed);

            BuildingGrid = new Building[World.AreaSize, World.AreaSize];

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

        public override string ToString()
            => $"{typeof (World).Name}; Pattern: {{{Pattern}}}, Seed: {Seed}, Type: {Type}";
    }
}

