using System;
using CommonStructures;
using GameCore.Modules.WorldModule.Land;
using GameCore.Modules.WorldModule.Buildings;

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

