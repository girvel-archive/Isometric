using System.Linq;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using Isometric.Vector;
using Newtonsoft.Json.Linq;

namespace Isometric.Server.Extensions
{
    public static class CommonHelper
    {
        public static JObject ToJson(this Area area)
        {
            return new JObject
            {
                ["Building grid"] = new JArray(area.BuildingGrid.Cast<Building>().Select(b => b.Pattern.Id)),
                ["Grid width"] = World.AreaSize,
                ["Grid height"] = World.AreaSize,
            };
        }



        public static JObject CreateUpgradeData(int id, IntVector position)
        {
            return new JObject
            {
                ["To"] = id,
                ["Position"] = JObject.FromObject(position),
            };
        }
    }
}

