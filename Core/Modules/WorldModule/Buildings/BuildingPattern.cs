using System;
using System.Collections.Generic;
using Isometric.CommonStructures;

namespace Isometric.Core.Modules.WorldModule.Buildings
{
    [Serializable]
    public class BuildingPattern
    {
        public delegate void BonusTickAction(Building building, ref Resources resources);

        public delegate bool UpgradeCondition(BuildingPattern previous, Building currentBuilding);



        public short ID { get; set; }

        public string Name { get; set; } 

        public BuildingType Type { get; set; }



        public Action<Building> TickIndependentAction { get; set; }

        public Func<Building, Resources> TickResourcesAction { get; set; }

        public BonusTickAction TickResourcesBonusAction { get; set; }



        public UpgradeCondition UpgradeAdditionalCondition { get; set; } 

        public Resources Price { get; set; }



        public Resources Resources { get; set; }

        public TimeSpan UpgradeTimeNormal { get; set; }



        private static short _nextID = 0;
        private static readonly List<BuildingPattern> Patterns = new List<BuildingPattern>();

        

        [Obsolete("using serialization ctor", true)]
        public BuildingPattern()
        {
            if (!Patterns.Contains(this))
            {
                Patterns.Add(this);
            }
        }

        public BuildingPattern(
            string name, Resources resources, Resources price, TimeSpan upgradeTimeNormal,
            BuildingType type = BuildingType.Nature)
        {
            Name = name;
            Resources = resources;
            Price = price;
            Type = type;
            UpgradeTimeNormal = upgradeTimeNormal;

            ID = _nextID++;
            Patterns.Add(this);
        }


        
        public static BuildingPattern Find(short id)
        {
            return Patterns.Find(p => p.ID == id);
        }

        public bool UpgradePossible(Resources playerResources, BuildingPattern previous, Building currentBuilding)
        {
            return (UpgradeAdditionalCondition?.Invoke(previous, currentBuilding) ?? true)
                && playerResources.Enough(Price);
        }
    }
}

