using System.Linq;
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
                new BuildingPattern(BuildingPatternNames.WoodHouse, DefaultBuildingGraph.Instance.Graph),
                new BuildingPattern(BuildingPatternNames.WoodHouse2, DefaultBuildingGraph.Instance.Graph),
                new BuildingPattern(BuildingPatternNames.Forest, DefaultBuildingGraph.Instance.Graph, type: BuildingType.Nature),
                new BuildingPattern(BuildingPatternNames.Plain, DefaultBuildingGraph.Instance.Graph, type: BuildingType.Nature),
                new BuildingPattern(BuildingPatternNames.Water, DefaultBuildingGraph.Instance.Graph, type: BuildingType.Nature),
                new BuildingPattern(BuildingPatternNames.Rock, DefaultBuildingGraph.Instance.Graph, type: BuildingType.Nature),
            };
        }



        public BuildingPattern Get(string name) => _buildingPatternCollection.Get(name);
    }
}