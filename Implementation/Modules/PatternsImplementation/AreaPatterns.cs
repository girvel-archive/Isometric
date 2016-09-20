using System;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using Isometric.Implementation.Modules.GameData;
using RandomExtensions;
using VectorNet;

namespace Isometric.Implementation.Modules.PatternsImplementation
{
    [Serializable]
    public static class AreaPatterns
    {
        public static AreaPattern Forest { get; }



        static AreaPatterns()
        {
            Forest = new AreaPattern(
                (terr, seed) => _defaultGeneration(terr, seed, new RandomCollection<BuildingPattern>(new Random(seed))
                {
                    new RandomPair<BuildingPattern>(6, GameDataManager.Instance.GetBuildingPattern("Forest")),
                    new RandomPair<BuildingPattern>(2, GameDataManager.Instance.GetBuildingPattern("Plain")),
                    new RandomPair<BuildingPattern>(2, GameDataManager.Instance.GetBuildingPattern("Water")),
                    new RandomPair<BuildingPattern>(1, GameDataManager.Instance.GetBuildingPattern("Rock")),
                }));
        }


    
        private static void _defaultGeneration(Area area, int seed, RandomCollection<BuildingPattern> chanceCollection)
        {
            for (var y = 0; y < World.Data.AreaSize; y++)
            {
                for (var x = 0; x < World.Data.AreaSize; x++)
                {
                    var pos = new IntVector(x, y); 

                    area[pos] = new Building(
                        pos, Player.Nature, area,
                        chanceCollection.Get());
                }
            }
        }
    }
}