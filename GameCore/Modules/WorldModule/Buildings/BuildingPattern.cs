using System;
using System.Collections.Generic;
using GameCore.Modules.PlayerModule;
using CompressedStructures;

namespace GameCore.Modules.WorldModule.Buildings
{
	[Serializable]
	public class BuildingPattern
	{
        public delegate void BonusTickAction(Building building, ref Resources resources);

		// TODO графику отдельно
		public Action<Building> TickIndependentAction { get; set; }

        public Func<Building, Resources> TickResourcesAction { get; set; }

        public BonusTickAction TickResourcesBonusAction { get; set; }

		public Func<BuildingPattern, Building, bool> ChangeCondition { get; set; } 
		public Resources NeedResources { get; set; } 

		public BuildingType Type { get; set; }

		public Resources Resources { get; set; }



        [Obsolete("using serialization ctor", true)]
		public BuildingPattern() {}

		public BuildingPattern(
			Resources resources, Resources needResources, BuildingType type = BuildingType.Nature)
		{
			Resources = resources;
			NeedResources = needResources;
			Type = type;
		}
	}
}

