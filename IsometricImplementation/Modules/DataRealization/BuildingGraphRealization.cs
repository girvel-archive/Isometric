using IsometricCore.Modules;
using IsometricCore.Modules.WorldModule.Buildings;
using IsometricCore.Structures;
using IsometricImplementation.Modules.PatternsRealization;

namespace IsometricImplementation.Modules.DataRealization
{
    internal static class BuildingGraphRealization
    {
        internal static void Init()
        {
            BuildingGraph.Instance = new Graph<BuildingPattern>(true);

            BuildingGraph.Instance.Root
                = new GraphNode<BuildingPattern>(
                    BuildingPatterns.WoodHouse,
                    BuildingGraph.Instance);

            BuildingGraph.Instance.Root.Add(
                new GraphNode<BuildingPattern>(
                    BuildingPatterns.WoodHouse2,
                    BuildingGraph.Instance));
        }
    }
}