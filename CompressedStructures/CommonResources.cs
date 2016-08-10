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
        


        public CommonResources(Dictionary<ResourceType, int> resource, Dictionary<ResourceType, int> lastIncrease)
        {
            Resource = resource;
            LastIncrease = lastIncrease;
        }

        [Obsolete("using serialization ctor", true)]        public CommonResources() {}
    }
}