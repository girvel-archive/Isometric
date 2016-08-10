using System;
using GameCore.Modules.PlayerModule;
using GameCore.Modules.TickModule;

namespace GameCore.Modules.WorldModule.Land
{
	[Serializable]
	public class TerritoryPattern
	{
		public ushort ID { get; }
		private static ushort _nextID;

		public Action<Territory, int> Generate { get; set; } 

		public Action<Territory, Player> GenerateVillage { get; set; }

		public Action<Territory> Tick { get; set; }



		public TerritoryPattern() {}

		public TerritoryPattern(
			Action<Territory, int> generate, 
			Action<Territory, Player> generateVillage)
			: this()
		{
			Tick = territory =>
			{
                foreach (IIndependentChanging building in territory.BuildingGrid)
				{
					building.Tick();
				}
			};

			Generate = generate;
			GenerateVillage = generateVillage;

			ID = _nextID++;
		}
	}
}

