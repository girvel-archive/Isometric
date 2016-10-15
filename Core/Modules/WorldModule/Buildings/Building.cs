using System;
using System.Linq;
using Isometric.CommonStructures;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule.Land;
using VectorNet;

namespace Isometric.Core.Modules.WorldModule.Buildings
{
    [Serializable]
    public class Building : IIndependentChanging, IResourcesChanging, IResourcesBonusChanging
    {
        public BuildingPattern Pattern { get; set; }



        public IntVector Position { get; set; }

        public Player Owner { get; set; }

        public Area Area { get; set; }



        public int PeopleNow { get; set; }

        public Resources Resources { get; set; }



        public bool Ready { get; set; }

        public DateTime UpgradeBeginTime { get; private set; }

        public TimeSpan UpgradeDuration { get; private set; }



        public Building() {}

        public Building(IntVector position, Player owner, Area territory, BuildingPattern pattern)
        {
            Position = position;
            Owner = owner;
            Area = territory;
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



        public bool TryUpgrade(BuildingPattern target, Player player)
        {
            var foundedObjects =
                Pattern.Graph
                    .GetNodes()
                    .Where(node => node.Value == Pattern
                                   && node.IsParentOf(target));

            if (!foundedObjects.Any()
                || !target.UpgradePossible(player.CurrentResources, Pattern, this)
                || !new[] {player, Player.Nature}.Contains(Owner))
            {
                return false;
            }
            Owner = player;

            Owner.CurrentResources -= target.Price;
            // TODO 1.1 upgrade duration (this.properties + ActionsProcessor)
            InitFromPattern(target);

            return true;
        }



        protected void InitFromPattern(BuildingPattern pattern)
        {
            Pattern = pattern;
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

        public override string ToString()
            => $"{typeof (Building).Name}; Pattern: {Pattern}, Position: {Position}, Owner: {Owner}";
    }
}

