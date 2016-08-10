using System;
using System.Collections.Generic;
using CompressedStructures;
using GameCore.Modules.WorldModule.Buildings;

namespace GameRealization.Main
{
    [Serializable]
    public static class PatternsRealization
    {
        public static BuildingPattern Plain { get; }
        public static BuildingPattern Rock { get; }
        public static BuildingPattern Water { get; }
        public static BuildingPattern Forest { get; }
        public static BuildingPattern WoodHouse { get; }
        public static BuildingPattern WoodHouse2 { get; }



        static PatternsRealization()
        {
            Plain = new BuildingPattern(
                "Plain", ConsoleColor.DarkGreen, '.',
                new Dictionary<ResourceType, int>(),
                new Dictionary<ResourceType, int>(),
                BuildingType.Space);

            Rock = new BuildingPattern(
                "Rock", ConsoleColor.DarkGray, 'r',
                new Dictionary<ResourceType, int> {
                    [ResourceType.Stone] = 1000,
                },
                new Dictionary<ResourceType, int>());

            Water = new BuildingPattern(
                "Water", ConsoleColor.DarkBlue, '~',
                new Dictionary<ResourceType, int>(),
                new Dictionary<ResourceType, int>());

            Forest = new BuildingPattern(
                "Forest", ConsoleColor.DarkGreen, 'f',
                new Dictionary<ResourceType, int>(), 
                new Dictionary<ResourceType, int>());

            WoodHouse = new BuildingPattern(
                "Wood house I", ConsoleColor.Yellow, 'H', 20,
                BuildingType.Building,
                new Dictionary<ResourceType, int>(), 
                new Dictionary<ResourceType, int>()) {
                    TickIndependentAction = args => { },
                };

            WoodHouse2 = new BuildingPattern(
                "Wood house II", ConsoleColor.Yellow, 'H', 30,
                BuildingType.Building,
                new Dictionary<ResourceType, int>(),
                new Dictionary<ResourceType, int>()) {
                    TickIndependentAction = args => { },
                };
        }
    }
}