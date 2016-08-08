using System;
using System.Collections.Generic;
using GameCore.Modules.PlayerModule;

namespace GameCore.Modules.WorldModule.Buildings
{
	[Serializable]
	public class BuildingPattern
	{
		// TODO графику отдельно
		public Action<Building> TickIndependentAction { get; set; } = args => { };
        public Func<Building, Resources> TickResourcesAction { get; set; } = args => { };
        public Func<Building, Resources, Resources> TickResourcesBonusAction { get; set; } = args => { };


		public Func<BuildingPattern, Building, bool> ChangeCondition { get; set; } 
		public Resources NeedResources { get; set; } 

		public BuildingType Type { get; set; }

		public Resources Resources { get; set; }



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

