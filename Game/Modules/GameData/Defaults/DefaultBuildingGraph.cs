using System.Linq;
using Girvel.Graph;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Game.Modules.GameData.Defaults
{
    internal class DefaultBuildingGraph
    {
        private static DefaultBuildingGraph _instance;
        public static DefaultBuildingGraph Instance => _instance ?? (_instance = new DefaultBuildingGraph());



        public Graph<BuildingPattern> Graph;



        private DefaultBuildingGraph()
        {
            Graph = new Graph<BuildingPattern>();
        }



        public void Initialize()
        {
            var plain = Graph.NewNode(BuildingPatternList.Instance.First(p => p.Name == BuildingPatternNames.Plain));
            var house1 = Graph.NewNode(BuildingPatternList.Instance.First(p => p.Name == BuildingPatternNames.WoodHouse));
            var house2 = Graph.NewNode(BuildingPatternList.Instance.First(p => p.Name == BuildingPatternNames.WoodHouse2));

            plain.AddChild(house1);
            plain.AddChild(house2);

            house1.AddChild(plain);
            house2.AddChild(plain);
        }
    }
}