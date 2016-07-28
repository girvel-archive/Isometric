using System;
using System.Collections.Generic;
using CompressedStructures;

namespace GameBasics.Buildings
{
    [Serializable]
    public class BuildingPattern
    {
        // TODO графику отдельно
        public string Name { get; set; }
        public ConsoleColor Color { get; set; }
        public char Symbol { get; set; }
        public Action<Building> RefreshAction { get; set; } = args => { };

        public Func<BuildingPattern, Building, bool> ChangeCondition { get; set; } 
        public Dictionary<ResourceType, int> NeedResources { get; set; } 

        public BuildingType Type { get; set; }

        public int PeopleMax { get; set; }
        public Dictionary<ResourceType, int> Resources { get; set; }



        public BuildingPattern(
            string name, 
            ConsoleColor color, 
            char symbol, 
            int peopleMax, 
            BuildingType type,
            Dictionary<ResourceType, int> resources,
            Dictionary<ResourceType, int> needResources)
            : this(name, color, symbol, needResources, needResources, type)
        {
            PeopleMax = peopleMax;
            Resources = resources;
        }

        public BuildingPattern(
            string name,
            ConsoleColor color,
            char symbol,
            Dictionary<ResourceType, int> resources,
            Dictionary<ResourceType, int> needResources,
            BuildingType type = BuildingType.Nature)
        {
            Name = name;
            Color = color;
            Symbol = symbol;
            Resources = resources;
            NeedResources = needResources;
            Type = type;
        }
    }
}