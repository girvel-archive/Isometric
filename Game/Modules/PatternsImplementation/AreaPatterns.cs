using System;
using System.Linq;
using Isometric.Core.Modules;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using RandomExtensions;
using VectorNet;

namespace Isometric.Game.Modules.PatternsImplementation
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
                    new RandomPair<BuildingPattern>(6, MainBuildingList.Instance.First(b => b.Name == "Forest")),
                }));
        }


    
        private static void _defaultGeneration(Area area, int seed, RandomCollection<BuildingPattern> chanceCollection)
        {
            for (var y = 0; y < World.AreaSize; y++)
            {
                for (var x = 0; x < World.AreaSize; x++)
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