using CommonStructures;
using IsometricCore.Modules.WorldModule.Land;

namespace VisualServer.Extensions
{
    public static class CommonExtensions
    {
        public static CommonTerritory ToCommon(this Territory territory)
        {
            return new CommonTerritory(territory.BuildingGrid.TwoDimSelect(b => b.Pattern.ID));
        }
    }
}

