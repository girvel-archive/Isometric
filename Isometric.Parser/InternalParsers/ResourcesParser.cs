using System;
using System.Text.RegularExpressions;
using Isometric.CommonStructures;

namespace Isometric.Parser.InternalParsers
{
    internal class ResourcesParser : IParser
    {
        private static ResourcesParser _instance;
        public static ResourcesParser Instance => _instance ?? (_instance = new ResourcesParser());
        
        private ResourcesParser() { }



        public Type Type => typeof (Resources);



        public bool TryParse(string str, object additionalData, out object obj)
        {
            obj = null;
            var result = new Resources();

            if (!Regex.IsMatch(str, @"^ *(\w*: *\d{1,},? *)*(\w*: *\d{1,})? *$"))
            {
                return false;
            }

            foreach (var name in typeof(ResourceType).GetEnumNames())
            {
                int resource;
                var resourceParts =
                    Regex.Match(str, name.ToLower() + @": \d*")
                         .ToString()
                         .Split(' ');

                if (resourceParts.Length == 2 && int.TryParse(resourceParts[1], out resource))
                {
                    result.ResourcesArray[(int)(ResourceType)Enum.Parse(typeof(ResourceType), name)] = resource;
                }
            }

            obj = result;
            return true;
        }

        public string GetValueString(object obj)
        {
            var resources = (Resources) obj;
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