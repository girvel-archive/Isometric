﻿using System;
using System.Collections.Generic;
using GameCore.Modules.PlayerModule;
using CommonStructures;

namespace GameCore.Modules.WorldModule.Buildings
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



        public UpgradeCondition UpgradePossible { get; set; } 

		public Resources NeedResources { get; set; }

        public Resources Resources { get; set; }

        public int UpgradeTimeNormal { get; set; }



        private static short _nextID = 0;





        [Obsolete("using serialization ctor", true)]
		public BuildingPattern() {}

		public BuildingPattern(
			string name, Resources resources, Resources needResources, int upgradeTimeNormal,
            BuildingType type = BuildingType.Nature)
		{
			Resources = resources;
			NeedResources = needResources;
			Type = type;
            UpgradeTimeNormal = upgradeTimeNormal;

            ID = _nextID++;
		}
	}
}

