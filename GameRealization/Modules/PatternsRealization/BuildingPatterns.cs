using System;
using System.Collections.Generic;
using CommonStructures;
using GameCore.Modules.WorldModule.Buildings;

namespace GameRealization.Modules.PatternsRealization
{
    [Serializable]
    public static class BuildingPatterns
    {
        public static BuildingPattern Plain { get; }
        public static BuildingPattern Rock { get; }
        public static BuildingPattern Water { get; }
        public static BuildingPattern Forest { get; }
        public static BuildingPattern WoodHouse { get; }
        public static BuildingPattern WoodHouse2 { get; }



        static BuildingPatterns()
        {
            Plain = new BuildingPattern(
                "Plain",
                new Resources(),
                new Resources(),
                BuildingType.Space);
            
            Rock = new BuildingPattern(
                "Rock",
                new Resources(stone: 10000),
                new Resources());

            Water = new BuildingPattern(
                "Water",
                new Resources(),
                new Resources());

            Forest = new BuildingPattern(
                "Forest",
                new Resources(wood: 1000),
                new Resources());

            WoodHouse = new BuildingPattern(
                "Wood house I",
                new Resources(),
                new Resources(wood: 500));

            WoodHouse2 = new BuildingPattern(
                "Wood house II", 
                new Resources(),
                new Resources(wood: 300, stone: 100));
        }
    }
}