using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule.Land;

namespace Isometric.Server.Extensions
{
    public static class CommonExtensions
    {
        public static CommonArea ToCommon(this Area territory)
        {
            return new CommonArea(territory.BuildingGrid.TwoDimSelect(b => b.Pattern.Id));
        }
    }
}

