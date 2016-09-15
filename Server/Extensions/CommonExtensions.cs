using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule.Land;

namespace Isometric.Server.Extensions
{
    public static class CommonExtensions
    {
        public static CommonTerritory ToCommon(this Territory territory)
        {
            return new CommonTerritory(territory.BuildingGrid.TwoDimSelect(b => b.Pattern.Id));
        }
    }
}

