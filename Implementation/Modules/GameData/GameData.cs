using System;
using Girvel.Graph;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Implementation.Modules.GameData
{
    [Serializable]
    public class GameData
    {
        public BuildingPattern[] Patterns { get; }

        public Graph<BuildingPattern> BuildingGraph { get; }



        public GameData(BuildingPattern[] patterns, Graph<BuildingPattern> graph)
        {
            Patterns = patterns;
            BuildingGraph = graph;
        }
    }
}