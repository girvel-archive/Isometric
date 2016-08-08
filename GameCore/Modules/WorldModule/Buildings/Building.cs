using System;
using VectorNet;
using GameCore.Modules.WorldModule.Interfaces;
using GameCore.Modules.PlayerModule;
using GameCore.Modules.WorldModule.Land;
using System.Collections.Generic;
using GameCore.Modules.TickModule;

namespace GameCore.Modules.WorldModule.Buildings
{
	[Serializable]
    public class Building : IIndependentChanging, IResourcesChanging, IResourcesBonusChanging
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



        public bool TryUpgrade(BuildingPattern target)
		{
			var foundedObjects = Owner.Game.BuildingGraph.Find(Pattern);

			if (!foundedObjects[0].IsParentOf(target)
                || !target.ChangeCondition?.Invoke(Pattern, this) ?? false
                || target.NeedResources.AllNotLessThan(Owner.CurrentResources))
			{
				return false;
			}

            Owner.CurrentResources -= target.NeedResources;
			InitFromPattern(target);

            return true;
		}



		protected void InitFromPattern(BuildingPattern pattern)
		{
			Resources = new Dictionary<ResourceType, int>(Pattern.Resources);
		}



        #region Interfaces

        void IIndependentChanging.Tick()
        {
            Pattern.TickIndependentAction?.Invoke(this);
        }

        Resources IResourcesChanging.Tick()
        {
            return Pattern.TickResourcesAction(this);
        }

        void IResourcesBonusChanging.Tick(ref Resources resources)
        {
            resources = Pattern.TickResourcesBonusAction(this, resources);
        }

        #endregion
	}
}

