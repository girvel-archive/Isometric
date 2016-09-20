using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using VectorNet;

namespace Isometric.Core.Modules.WorldModule
{
    public class WorldData
    {
        public WorldData() {}



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



        public short AreaSize;
        public IntVector AreaVectorSize { get; private set; }

        public DefaultBuilding[] StartBuildings;



        /// <summary>
        /// Generates on existing territory player's village
        /// </summary>
        public VillageGenerator NewPlayerArea { get; set; }

        public delegate void VillageGenerator(Player owner, Area territory);

        /// <summary>
        /// Creates new territory
        /// </summary>
        public AreaGenerator GenerateArea { get; set; } 

        public delegate Area AreaGenerator(Area[,] landGrid, int x, int y, int seed);



        public void RefreshDependentValues()
        {
            AreaVectorSize = new IntVector(AreaSize, AreaSize);
        }
    }
}

