using System;
using VectorNet;
using System.Collections.Generic;
using System.Linq;
using GameCore.Modules.WorldModule.Land;
using GameCore.Modules.WorldModule.Buildings;
using GameCore.Modules.PlayerModule;
using GameCore.Modules;
using GameCore.Modules.WorldModule;

namespace GameRealization.Modules.PatternsRealization
{
    [Serializable]
    public static class TerritoryPatterns
    {
        public static TerritoryPattern Forest { get; }



        static TerritoryPatterns()
        {
            Forest = new TerritoryPattern(
                (terr, seed) => _defaultGeneration(terr, seed, new Dictionary<int, BuildingPattern> { 
                    [1] = BuildingPatterns.Forest,
                }));
        }


    
        private static void _defaultGeneration(
            Territory territory, 
            int seed, 
            Dictionary<int, BuildingPattern> chanceCollection)
        {
            var random = new Random(seed);

            for (var y = 0; y < World.Data.TerritorySize; y++)
            {
                for (var x = 0; x < World.Data.TerritorySize; x++)
                {
                    var pos = new IntVector(x, y); 

                    territory[pos] = new Building(
                        pos, Player.Nature, territory,
                        _getRandomBuildingPattern(random, chanceCollection));
                }
            }
        }

        // TODO chance library
        private static BuildingPattern _getRandomBuildingPattern(
            Random random, Dictionary<int, BuildingPattern> chanceCollection)
        {
            var sum = chanceCollection.Sum(pair => pair.Key);
            var chanceResult = random.NextDouble() * sum;

            foreach (var chanceElement in chanceCollection)
            {
                chanceResult -= chanceElement.Key;
            
                if (chanceResult <= 0)
                {
                    return chanceElement.Value;
                }
            }

            throw new NotImplementedException(
                "_getRandomBuildingPattern: something went wrong");
        }
    }
}