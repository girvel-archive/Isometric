using System;
using System.Linq;
using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Implementation.Modules.GameData.Defaults
{
    internal class DefaultBuildings
    {
        private static DefaultBuildings _instance;
        public static DefaultBuildings Instance => _instance ?? (_instance = new DefaultBuildings());



        private readonly BuildingPatternCollection _buildingPatternCollection;

        public BuildingPattern[] GetPatterns() => _buildingPatternCollection.ToArray();



        private DefaultBuildings()
        {
            _buildingPatternCollection = new BuildingPatternCollection
            {
                new BuildingPattern(BuildingNames.WoodHouse, new Resources(), new Resources(), new TimeSpan(), BuildingType.Building),
                new BuildingPattern(BuildingNames.WoodHouse2, new Resources(), new Resources(), new TimeSpan(), BuildingType.Building),
                new BuildingPattern(BuildingNames.Forest, new Resources(), new Resources(), new TimeSpan(), BuildingType.Nature),
                new BuildingPattern(BuildingNames.Plain, new Resources(), new Resources(), new TimeSpan(), BuildingType.Nature),
                new BuildingPattern(BuildingNames.Water, new Resources(), new Resources(), new TimeSpan(), BuildingType.Nature),
                new BuildingPattern(BuildingNames.Rock, new Resources(), new Resources(), new TimeSpan(), BuildingType.Nature),
            };
        }
    }
}