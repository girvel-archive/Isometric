using System;
using System.Collections.Generic;
using System.Linq;
using Girvel.Graph;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using Isometric.Vector;
using Newtonsoft.Json.Linq;

namespace Isometric.Client.Extensions
{
    public static class CommonHelper
    {
        public static JObject ToJson(this Area area)
        {
            return new JObject
            {
                ["Grid"] = new JArray(area.BuildingGrid.Cast<Building>().Select(b => b.Pattern.Id)),
                ["Width"] = World.AreaSize,
                ["Height"] = World.AreaSize,
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



        public static JObject CreateContextActionsData(
            IEnumerable<GraphNode<BuildingPattern>> children,
            Func<GraphNode<BuildingPattern>, bool> possibleSelector,
            Func<GraphNode<BuildingPattern>, string> textSelector,
            Func<GraphNode<BuildingPattern>, int> idSelector)
        {
            return new JObject
            {
                ["Actions"] = new JArray(
                    children
                        .Select(
                            c => new JObject
                            {
                                ["Possible"] = possibleSelector(c),
                                ["Text"] = textSelector(c),
                                ["To"] = idSelector(c),
                            }))
            };
        }
    }
}

