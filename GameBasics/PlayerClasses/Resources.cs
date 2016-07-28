using System;
using System.Collections.Generic;
using System.Linq;
using CompressedStructures;

namespace GameBasics.PlayerClasses
{
    [Serializable]
    public class Resources : IRefreshable
    {
        public Dictionary<ResourceType, int> Resource { get; set; }
        public Dictionary<ResourceType, int> LastIncrease { get; set; } 

        public Player Owner { get; }



        public Resources(Player owner)
        {
            Owner = owner;
            Resource = new Dictionary<ResourceType, int> {
                [ResourceType.Corn] = 0,
                [ResourceType.Meat] = 0,
                [ResourceType.Wood] = 0,
                [ResourceType.Gold] = 0,
                [ResourceType.Stone] = 0,
                [ResourceType.People] = 0,
                [ResourceType.Progress] = 0,
            };
            LastIncrease = new Dictionary<ResourceType, int> {
                [ResourceType.Corn] = 0,
                [ResourceType.Meat] = 0,
                [ResourceType.Wood] = 0,
                [ResourceType.Gold] = 0,
                [ResourceType.Stone] = 0,
                [ResourceType.People] = 0,
                [ResourceType.Progress] = 0,
            };
        }



        public void Refresh()
        {
            
        }

        public bool Enough(Dictionary<ResourceType, int> need)
        {
            return !need.Any(pair => pair.Value > Resource[pair.Key]);
        }

        public void Deduct(Dictionary<ResourceType, int> resources)
        {
            foreach (var pair in resources)
            {
                Resource[pair.Key] -= pair.Value;
            }
        }
    }
}

