using System;
using VectorNet;
using GameCore.Modules.WorldModule.Buildings;

namespace GameCore.Modules.WorldModule
{
    public class WorldData
	{
        public WorldData() {}



        public struct DefaultBuilding
        {
            public int Numbers;
            public BuildingPattern Pattern;

            public DefaultBuilding(int numbers, BuildingPattern pattern)
            {
                Numbers = numbers;
                Pattern = pattern;
            }
        }



        public short TerritorySize;
        public IntVector TerritoryVectorSize { get; private set; }

        public DefaultBuilding[] StartBuildings; 



        public void RefreshDependentValues()
        {
            TerritoryVectorSize = new IntVector(TerritorySize, TerritorySize);
        }
	}
}

