using System;

namespace GameCore.Modules.WorldModule.Land
{
	[Serializable]
	public class Territory
	{
		public TerritoryPattern Pattern { get; set; }

		public Building[,] BuildingGrid { get; }



		public Territory() {}
	}
}

