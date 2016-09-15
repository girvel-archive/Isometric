﻿using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Structures;
using Isometric.Implementation.Modules.PatternsImplementation;

namespace Isometric.Implementation.Modules.DataImplementation
{
    internal static class BuildingGraphImplementation
    {
        internal static void Init()
        {
            var g = BuildingGraph.Instance = new Graph<BuildingPattern>(true);

            g.SetRoot(BuildingPatterns.Plain);

            g.Root
                .Add(BuildingPatterns.WoodHouse)
                .Add(BuildingPatterns.WoodHouse2);

            GraphNode.Create(BuildingPatterns.Forest, g)
                .Add(g.Root);

            GraphNode.Create(BuildingPatterns.Water, g)
                .Add(g.Root);
        }
    }
}