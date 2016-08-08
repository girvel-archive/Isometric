using System;
using GameBasics;
using GameBasics.Buildings;
using GameBasics.Structures;

namespace GameRealization.Main
{
    [Serializable]
    public class GameRealization : Game
    {
        public GameRealization(int seed) : base(seed)
        {
            World = new WorldRealization(seed);
            BuildingGraph = new Graph<BuildingPattern>();

            var woodHouseNode = new GraphNode<BuildingPattern>(PatternsRealization.WoodHouse, BuildingGraph);
            var woodHouse2Node = new GraphNode<BuildingPattern>(PatternsRealization.WoodHouse2, BuildingGraph);

            BuildingGraph.Root = new GraphNode<BuildingPattern>(PatternsRealization.Plain, BuildingGraph);
            BuildingGraph.Root.Add(woodHouseNode);
            BuildingGraph.Root.Add(woodHouse2Node);
        }
    }
}