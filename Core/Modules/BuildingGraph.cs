using System;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Structures;

namespace Isometric.Core.Modules
{
    [Serializable]
    public class BuildingGraph
    {
        public static Graph<BuildingPattern> Instance { get; set; }
    }
}

