using VectorNet;
using IsometricCore.Modules.WorldModule.Buildings;
using IsometricCore.Modules.PlayerModule;
using IsometricCore.Modules.WorldModule.Land;

namespace IsometricCore.Modules.WorldModule
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



        public short TerritorySize;
        public IntVector TerritoryVectorSize { get; private set; }

        public DefaultBuilding[] StartBuildings;



        /// <summary>
        /// Generates on existing territory player's village
        /// </summary>
        public VillageGenerator NewPlayerTerritory { get; set; }

        public delegate void VillageGenerator(Player owner, Territory territory);

        /// <summary>
        /// Creates new territory
        /// </summary>
        public TerritoryGenerator GenerateTerritory { get; set; } 

        public delegate Territory TerritoryGenerator(Territory[,] landGrid, int x, int y, int seed);



        public void RefreshDependentValues()
        {
            TerritoryVectorSize = new IntVector(TerritorySize, TerritorySize);
        }
    }
}

