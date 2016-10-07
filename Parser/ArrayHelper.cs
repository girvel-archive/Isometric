using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Isometric.Parser
{
    public static class ArrayHelper
    {
        public static string[] ParseArray(this string str)
        {
            return
                Regex.Match(str, @"^((""([^""]|\\"")*""),?\s*)*$").Groups[1].Captures
                    .Cast<Capture>()
                    .Select(c => Regex.Replace(c.Value, @"\\.", match => match.Value[1].ToString()))
                    .ToArray();
        }
    }
}