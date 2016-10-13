using System;
using System.Linq;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.SettingsModule;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using VectorNet;

namespace Isometric.Core.Modules.WorldModule
{
    [Serializable]
    public class World : IIndependentChanging
    {
        [GameConstant]
        public static int AreaSize { get; set; }
        
        public static IntVector AreaVectorSize => new IntVector(AreaSize, AreaSize);

        /// <summary>
        /// Generates new area
        /// </summary>
        [GameConstant]
        public static AreaGenerator GenerateArea { get; set; }

        [GameConstant]
        public static DefaultBuilding[] StartBuildings { get; set; }
        
        /// <summary>
        /// Generates on existing territory player's village
        /// </summary>
        [GameConstant]
        public static VillageGenerator NewPlayerVillage { get; set; }



        public delegate Area AreaGenerator(Area[,] landGrid, int x, int y, int seed);

        public delegate void VillageGenerator(Player owner, Area territory);



        public World(int seed) 
        {
            LandGrid = new Area[AreaSize, AreaSize];

            Seed = seed;
            Random = new Random(Seed);
        }



        public Area[,] LandGrid { get; }

        public int Seed { get; }

        protected Random Random;



        public void CheckVersion()
        {

        }

        public Area LazyGetArea(IntVector position) => LazyGetArea(position.X, position.Y);

        public Area LazyGetArea(int x, int y)
        {
            return LandGrid[x, y] ?? (LandGrid[x, y] = GenerateArea(LandGrid, x, y, SeedForPosition(x, y)));
        }

        public Area NewPlayerArea(Player player)
        {
            Area result;
            do
            {
                result = LazyGetArea(SingleRandom.Next(AreaVectorSize));
            }
            while (result.Type != AreaGenerationType.Wild);

            NewPlayerVillage(player, result);
            result.Type = AreaGenerationType.Wild;

            return result;
        }



        public void Tick()
        {
            foreach (var territory in LandGrid.Cast<IIndependentChanging>().Where(territory => territory != null))
            {
                territory.Tick();
            }
        }


        // TODO F test it
        protected int SeedForPosition(int x, int y)
        {
            return (int)((decimal)Seed * (x * AreaSize + y) / (decimal)Math.Pow(AreaSize, 2));
        }


        public override string ToString() => $"{typeof (World).Name}; Seed: {Seed}";



        public struct DefaultBuilding
        {
            public int Number;
            public BuildingPattern Pattern;

            public DefaultBuilding(int number, BuildingPattern pattern)
            {
                Number = number;
                Pattern = pattern;
            }
        }
    }
}

