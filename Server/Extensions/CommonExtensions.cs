using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Land;
using Newtonsoft.Json.Linq;

namespace Isometric.Server.Extensions
{
    public static class CommonExtensions
    {
        public static string ToCommon(this Area area)
        {
            var jGrid = new JArray();
            foreach (var building in area.BuildingGrid)
            {
                jGrid.Add(building.Pattern.Id);
            }

            return new JObject
            {
                ["Building grid"] = jGrid,
                ["Grid width"] = World.AreaSize,
                ["Grid height"] = World.AreaSize,
            }.ToString();
        }
    }
}

