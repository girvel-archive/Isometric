using System;
using System.Linq;
using System.Text.RegularExpressions;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using RandomExtensions;

namespace Isometric.Editor.Extensions
{
    public static class RandomCollectionHelper
    {
        public static bool TryParse(this string str, out RandomCollection<BuildingPattern> result)
        {
            result = null;

            if (!Regex.IsMatch(str, @"^([\w\d\s.]*:\s*\d*,?\s*)*$"))
            {
                return false;
            }

            var resultCollection = new RandomCollection<BuildingPattern>();

            foreach (var pattern in GameData.Instance.BuildingPatterns)
            {
                var matches = Regex.Matches(str, pattern.Name + @":\s*(\d*)");

                if (matches.Count <= 0)
                {
                    continue;
                }

                if (matches.Count != 1)
                {
                    return false;
                }

                int value;
                if (!int.TryParse(matches[0].Groups[1].Value, out value))
                {
                    return false;
                }

                resultCollection.Add(new RandomPair<BuildingPattern>(value, pattern));
            }

            result = new RandomCollection<BuildingPattern>(resultCollection.ToArray());

            return true;
        }
    }
}