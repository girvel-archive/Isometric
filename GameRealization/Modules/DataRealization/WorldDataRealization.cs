using System;
using GameCore.Modules.WorldModule;

namespace GameRealization.Modules.DataRealization
{
    internal static class WorldDataRealization
	{
        static WorldDataRealization ()
		{
            WorldData.Instance = new WorldData() 
            {
                TerritorySize = 32,
            };
		}
	}
}

