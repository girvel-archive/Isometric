using System;
using System.Collections.Generic;
using Girvel.Graph;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.GameDataTools
{
    [Serializable]
    public class GameDataContainer
    {
        public BuildingPattern[] Patterns { get; }

        public Graph<BuildingPattern> BuildingGraph { get; }

        public Dictionary<string, object> Constants { get; }



        public GameDataContainer(BuildingPattern[] patterns, Graph<BuildingPattern> graph, Dictionary<string, object> constants)
        {
            Patterns = patterns;
            BuildingGraph = graph;
            Constants = constants;
        }
    }
}