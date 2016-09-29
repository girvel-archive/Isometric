using System;
using System.Text.RegularExpressions;
using Isometric.CommonStructures;

namespace Isometric.Parser.InternalParsers
{
    internal class ResourcesParser : IParser<Resources>
    {
        private static ResourcesParser _instance;
        public static ResourcesParser Instance => _instance ?? (_instance = new ResourcesParser());


        private ResourcesParser() { }



        public bool TryParse(string str, out Resources result)
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
                    result.ResourcesArray[(int)(ResourceType)Enum.Parse(typeof(ResourceType), type)] = resource;
                }
            }

            return true;
        }

        public string GetValueString(Resources resources)
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
                    result += $"{(result == "" ? "" : ", ")}{((ResourceType)i).ToString().ToLower()}: {resource}";
                }

                i++;
            }

            return result;
        }
    }
}