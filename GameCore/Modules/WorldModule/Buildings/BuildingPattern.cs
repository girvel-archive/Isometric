using System;
using System.Collections.Generic;

namespace GameCore.Modules.WorldModule.Buildings
{
	[Serializable]
	public class BuildingPattern
	{
		// TODO графику отдельно
		public Action<Building> RefreshAction { get; set; } = args => { };

		public Func<BuildingPattern, Building, bool> ChangeCondition { get; set; } 
		public Dictionary<ResourceType, int> NeedResources { get; set; } 

		public BuildingType Type { get; set; }

		public Dictionary<ResourceType, int> Resources { get; set; }



		public BuildingPattern() {}

		public BuildingPattern(
			Dictionary<ResourceType, int> resources,
			Dictionary<ResourceType, int> needResources,
			BuildingType type = BuildingType.Nature)
		{
			Resources = resources;
			NeedResources = needResources;
			Type = type;
		}
	}
}

