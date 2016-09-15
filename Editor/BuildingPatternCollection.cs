using System.Collections.Generic;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Editor
{
    public static class BuildingPatternCollection
    {
        public static List<BuildingPattern> Instance { get; set; }
            = new List<BuildingPattern>();
    }
}