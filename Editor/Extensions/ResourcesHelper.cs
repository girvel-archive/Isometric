using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Isometric.CommonStructures;

namespace Isometric.Editor.Extensions
{
    public static class ResourcesHelper
    {
        public static bool TryParse(this string str, out Resources result)
        {
            result = new Resources();

            if (!Regex.IsMatch(str, @"^ *(\w*: *\d{1,},? *)*(\w*: *\d{1,})? *$"))
            {
                return false;
            }
            
            foreach (var type in typeof(ResourceType).GetEnumNames())
            {
                int resource;
                var resourceParts =
                    Regex.Match(str, type.ToLower() + @": \d*")
                        .ToString()
                        .Split(' ');

                if (resourceParts.Length == 2 && int.TryParse(resourceParts[1], out resource))
                {
                    result.ResourcesArray[(int) (ResourceType) Enum.Parse(typeof(ResourceType), type)] = resource;
                }
            }

            return true;
        }

        public static string GetValueString(this Resources resources)
        {
            if (resources.Empty)
            {
                return "";
            }

            var i = 0;
            var result = string.Empty;

            foreach (var resource in resources.ResourcesArray)
            {
                if (resource != 0)
                {
                    result += $"{(result == "" ? "" : ", ")}{((ResourceType) i).ToString().ToLower()}: {resource}";
                }

                i++;
            }

            return result;
        }
    }
}