using Girvel.Graph;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Implementation.Modules.PatternsImplementation;

namespace Isometric.Implementation.Modules.DataImplementation
{
    internal static class BuildingGraphImplementation
    {
        internal static void Init()
        {
            GraphNode<BuildingPattern> woodHouse1;
            var g = BuildingGraph.Instance = new Graph<BuildingPattern>();

            var plain = g.NewNode(BuildingPatterns.Plain);

            plain.AddChild(woodHouse1 = g.NewNode(BuildingPatterns.WoodHouse));

            woodHouse1.AddChild(g.NewNode(BuildingPatterns.WoodHouse2));

            g.NewNode(BuildingPatterns.Forest).AddChild(plain);
            g.NewNode(BuildingPatterns.Water).AddChild(plain);
        }
    }
}