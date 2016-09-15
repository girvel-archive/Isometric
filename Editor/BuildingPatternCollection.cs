using System.Collections.Generic;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Editor
{
    public class BuildingPatternCollection
    {
        public static BuildingPatternCollection Instance { get; } 
            = new BuildingPatternCollection();

        public List<BuildingPattern> CurrentPatterns { get; }
            = new List<BuildingPattern>();
    }
}