using System;

namespace GameCore.Modules.WorldModule.Land
{
	[Serializable]
	public class TerritoryPattern
	{
		public ushort ID { get; }
		private static ushort _nextID;

		public Action<Territory, int> Generate { get; set; } 

		public Action<Territory, Player> GenerateVillage { get; set; }

		public Action<Territory> Refresh { get; set; }



		public TerritoryPattern() {}

		public TerritoryPattern(
			Action<Territory, int> generate, 
			Action<Territory, Player> generateVillage)
			: this()
		{
			Refresh = territory =>
			{
				foreach (var building in territory)
				{
					building.Refresh();
				}
			};

			Generate = generate;
			GenerateVillage = generateVillage;

			ID = _nextID++;
		}
	}
}

