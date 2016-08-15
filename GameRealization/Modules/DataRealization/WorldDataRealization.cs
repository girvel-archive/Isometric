using System;
using GameCore.Modules.WorldModule;

namespace GameRealization.Modules.DataRealization
{
    internal static class WorldDataRealization
	{
        static WorldDataRealization ()
		{
            World.Data = new WorldData() 
            {
                TerritorySize = 32,
            };
		}
	}
}

