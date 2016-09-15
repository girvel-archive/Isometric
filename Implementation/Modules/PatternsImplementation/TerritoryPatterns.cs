using System;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using RandomExtensions;
using VectorNet;

namespace Isometric.Implementation.Modules.PatternsImplementation
{
    [Serializable]
    public static class TerritoryPatterns
    {
        public static TerritoryPattern Forest { get; }



        static TerritoryPatterns()
        {
            Forest = new TerritoryPattern(
                (terr, seed) => _defaultGeneration(terr, seed, new RandomCollection<BuildingPattern>(new Random(seed))
                { 
                    new RandomPair<BuildingPattern>(6, BuildingPatterns.Forest),
                    new RandomPair<BuildingPattern>(2, BuildingPatterns.Plain),
                    new RandomPair<BuildingPattern>(2, BuildingPatterns.Water),
                    new RandomPair<BuildingPattern>(1, BuildingPatterns.Rock),
                }));
        }


    
        private static void _defaultGeneration(
            Territory territory, 
            int seed,
            RandomCollection<BuildingPattern> chanceCollection)
        {
            for (var y = 0; y < World.Data.TerritorySize; y++)
            {
                for (var x = 0; x < World.Data.TerritorySize; x++)
                {
                    var pos = new IntVector(x, y); 

                    territory[pos] = new Building(
                        pos, Player.Nature, territory,
                        chanceCollection.Get());
                }
            }
        }
    }
}