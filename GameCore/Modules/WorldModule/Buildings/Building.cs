using System;
using VectorNet;
using GameCore.Modules.WorldModule.Interfaces;
using GameCore.Modules.PlayerModule;
using GameCore.Modules.WorldModule.Land;
using System.Collections.Generic;
using GameCore.Modules.TickModule;
using CommonStructures;
using System.Linq;

namespace GameCore.Modules.WorldModule.Buildings
{
    [Serializable]
    public class Building : IIndependentChanging, IResourcesChanging, IResourcesBonusChanging
    {
        public BuildingPattern Pattern { get; set; }



        public IntVector Position { get; set; }

        public Player Owner { get; set; }

        public Territory Territory { get; set; }



        public int PeopleNow { get; set; }

        public Resources Resources { get; set; }



        public bool Ready { get; set; }

        public DateTime UpgradeBeginTime { get; private set; }

        public TimeSpan UpgradeDuration { get; private set; }



        public Building() {}

        public Building(IntVector position, Player owner, Territory territory, BuildingPattern pattern)
        {
            Position = position;
            Owner = owner;
            Territory = territory;
            Pattern = pattern;

            InitFromPattern(pattern);

            if (!Owner.GetOwnedBuildings()?.Contains(this) ?? false)
            {
                Owner.AddOwnedBuilding(this);
            }
        }

        ~Building()
        {
            if (Owner.GetOwnedBuildings()?.Contains(this) ?? false)
            {
                Owner.RemoveOwnedBuilding(this);
            }
        }



        public bool TryUpgrade(BuildingPattern target)
        {
            var foundedObjects = BuildingGraph.Instance.Find(Pattern);

            if (!(foundedObjects[0].IsParentOf(target)
                && (target.UpgradePossible?.Invoke(Pattern, this) ?? true)
                && target.NeedResources.Enough(Owner.CurrentResources)))
            {
                return false;
            }

            Owner.CurrentResources -= target.NeedResources;
            // TODO 1.1 upgrade duration (this.properties + ActionsProcessor)
            InitFromPattern(target);

            return true;
        }



        protected void InitFromPattern(BuildingPattern pattern)
        {
            Resources = Pattern.Resources;
        }



        #region Interfaces

        void IIndependentChanging.Tick()
        {
            Pattern.TickIndependentAction?.Invoke(this);
        }

        Resources IResourcesChanging.Tick()
        {
            return Pattern.TickResourcesAction?.Invoke(this) ?? new Resources();
        }

        void IResourcesBonusChanging.Tick(ref Resources resources)
        {
            Pattern.TickResourcesBonusAction?.Invoke(this, ref resources);
        }

        #endregion
    }
}

