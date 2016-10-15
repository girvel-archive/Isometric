using Girvel.Graph;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Game.Modules
{
    public static class SingleBuildingGraph
    {
        public static Graph<BuildingPattern> Instance { get; set; }
    }
}