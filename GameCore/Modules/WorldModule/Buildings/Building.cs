using System;
using VectorNet;
using GameCore.Modules.WorldModule.Interfaces;

namespace GameCore.Modules.WorldModule.Buildings
{
	[Serializable]
	public class Building : IRefreshable
	{
		public BuildingPattern Pattern { get; set; }

		public IntVector Position { get; }
		public Player Owner { get; set; }
		public Territory CurrentTerritory { get; private set; }

		public int PeopleNow { get; set; }

		public Dictionary<ResourceType, int> Resources { get; set; }



		public Building() {}

		public Building(IntVector position, Player owner, Territory territory, BuildingPattern pattern)
		{
			Position = position;
			Owner = owner;
			CurrentTerritory = territory;
			Pattern = pattern;

			InitFromPattern(pattern);

			if (!Owner?.OwnedBuildings?.Contains(this) ?? false)
			{
				Owner.OwnedBuildings.Add(this);
			}
		}

		~Building()
		{
			if (Owner?.OwnedBuildings?.Contains(this) ?? false)
			{
				Owner.OwnedBuildings.Remove(this);
			}
		}



		public void Refresh()
		{
			Pattern.RefreshAction?.Invoke(this);
		}

		public void TryUpgrade(BuildingPattern target)
		{
			var foundedObjects = Owner.Game.BuildingGraph.Find(Pattern);

			if (foundedObjects.Length > 1)
			{ 
				// TODO replace by bool field
				throw new Exception("building graph can't contain one object twice");
			}

			if (!foundedObjects[0].IsParentOf(target))
			{
				return false;
			}

			if (!Pattern.ChangeCondition?.Invoke(Pattern, this) ?? false)
			{
				return false;
			}

			if (Pattern.NeedResources.Any(
				resourcePair => Owner.CurrentResources.Resource[resourcePair.Key] < resourcePair.Value))
			{
				return false; // TODO определение причины на стороне клиента
			}

			foreach (var resourcePair in Pattern.NeedResources)
			{
				Owner.CurrentResources.Resource[resourcePair.Key] -= resourcePair.Value;
			}

			InitFromPattern(target);
		}



		protected void InitFromPattern(BuildingPattern pattern)
		{
			Resources = new Dictionary<ResourceType, int>(Pattern.Resources);
		}
	}
}

