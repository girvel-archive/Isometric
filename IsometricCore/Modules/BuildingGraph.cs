using System;
using IsometricCore.Structures;
using IsometricCore.Modules.WorldModule.Buildings;

namespace IsometricCore.Modules
{
    [Serializable]
    public class BuildingGraph
    {
        public static Graph<BuildingPattern> Instance { get; set; }
    }
}

