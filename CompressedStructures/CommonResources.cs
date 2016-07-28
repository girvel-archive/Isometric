using System;
using System.Collections.Generic;
using System.Text;
using CommandInterface;

namespace CompressedStructures
{
    [Serializable]
    public class CommonResources : ICompressable
    {
        public Dictionary<ResourceType, int> Resource { get; set; }
        public Dictionary<ResourceType, int> LastIncrease { get; set; }



        public string GetString => $"[{Resource.ToArgumentDictionary()};{LastIncrease.ToArgumentDictionary()}]";

        public byte[] GetBytes => Encoding.ASCII.GetBytes(GetString);
        


        public CommonResources(Dictionary<ResourceType, int> resource, Dictionary<ResourceType, int> lastIncrease)
        {
            Resource = resource;
            LastIncrease = lastIncrease;
        }

        public CommonResources() {}



        public static CommonResources GetFromString(string @string)
        {
            var data = @string.ParseType("[resource;last-increase]");
            return new CommonResources(
                data["resource"].ParseDictionary(sv => (ResourceType) Enum.Parse(typeof(ResourceType), sv), int.Parse), 
                data["last-increase"].ParseDictionary(sv => (ResourceType) Enum.Parse(typeof(ResourceType), sv), int.Parse));
        }

        public static CommonResources GetFromBytes(byte[] bytes)
            => GetFromString(Encoding.ASCII.GetString(bytes));
    }
}