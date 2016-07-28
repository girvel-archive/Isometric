using System;
using GameBasics;
using GameBasics.Buildings;
using VectorNet;
using VisualConsole;
using System.Collections.Generic;
using System.Linq;

namespace GameRealization
{
    [Serializable]
    public static class TerritoryPatterns
    {
        public static TerritoryPattern Forest { get; }



        static TerritoryPatterns()
        {
            Forest = new TerritoryPattern(
                'f', ConsoleColor.DarkGreen, 
                (terr, seed) => _defaultGeneration(terr, seed, 
                    new Dictionary<int, BuildingPattern> 
                    {
                        [1] = PatternsRealization.Forest,
                    }),
                _defaultVillageGeneration);
        }



        private static void _defaultVillageGeneration(Territory territory, Player player)
        {
            for (var i = 0; i < Territory.VillageHouses; i++)
            {
                var randomPosition = new IntVector(
                    BasicsHelper.MainRandom.Next(territory.SizeX),
                    BasicsHelper.MainRandom.Next(territory.SizeY));

                if (territory[randomPosition]?.Pattern 
                    != PatternsRealization.WoodHouse)
                {
                    territory[randomPosition] = new Building(
                        randomPosition, 
                        player, 
                        territory, 
                        PatternsRealization.WoodHouse);
                }
            }
        }
    
        private static void _defaultGeneration(
            Territory territory, 
            int seed, 
            Dictionary<int, BuildingPattern> chanceCollection)
        {
            var random = new Random(seed);

            for (var y = 0; y < territory.SizeX; y++)
            {
                for (var x = 0; x < territory.SizeY; x++)
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