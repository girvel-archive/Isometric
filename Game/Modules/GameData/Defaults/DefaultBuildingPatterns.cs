using System;
using System.Linq;
using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Game.Modules.GameData.Defaults
{
    internal class DefaultBuildingPatterns
    {
        private static DefaultBuildingPatterns _instance;
        public static DefaultBuildingPatterns Instance => _instance ?? (_instance = new DefaultBuildingPatterns());



        private readonly BuildingPatternCollection _buildingPatternCollection;

        public BuildingPattern[] GetPatterns() => _buildingPatternCollection.ToArray();



        private DefaultBuildingPatterns()
        {
            _buildingPatternCollection = new BuildingPatternCollection
            {
                new BuildingPattern(BuildingPatternNames.WoodHouse, new Resources(), new Resources(), new TimeSpan(), BuildingType.Building),
                new BuildingPattern(BuildingPatternNames.WoodHouse2, new Resources(), new Resources(), new TimeSpan(), BuildingType.Building),
                new BuildingPattern(BuildingPatternNames.Forest, new Resources(), new Resources(), new TimeSpan(), BuildingType.Nature),
                new BuildingPattern(BuildingPatternNames.Plain, new Resources(), new Resources(), new TimeSpan(), BuildingType.Nature),
                new BuildingPattern(BuildingPatternNames.Water, new Resources(), new Resources(), new TimeSpan(), BuildingType.Nature),
                new BuildingPattern(BuildingPatternNames.Rock, new Resources(), new Resources(), new TimeSpan(), BuildingType.Nature),
            };
        }



        public BuildingPattern Get(string name) => _buildingPatternCollection.Get(name);
    }
}