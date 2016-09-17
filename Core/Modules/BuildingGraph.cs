using System;
using Girvel.Graph;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Core.Modules
{
    [Serializable]
    public static class BuildingGraph
    {
        public static Graph<BuildingPattern> Instance { get; set; }
    }
}

