using System;
using System.Linq;
using Isometric.Core.Modules;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using RandomExtensions;
using Isometric.Vector;

namespace Isometric.Game.Modules.GameData.Defaults
{
    internal class DefaultAreaPatterns
    {
        private static DefaultAreaPatterns _instance;
        public static DefaultAreaPatterns Instance => _instance ?? (_instance = new DefaultAreaPatterns());



        public AreaPattern[] Areas { get; }



        private DefaultAreaPatterns()
        {
            Areas = new[]
            {
                new AreaPattern(AreaPatternNames.Forest, _generateForest)
            };
        }



        private void _generateForest(Area area, int seed)
        {
            GenerateDefaultArea(
                area,
                new RandomCollection<BuildingPattern>(
                    new Random(seed), 
                    new RandomPair<BuildingPattern>(1, BuildingPatternList.Instance.First(p => p.Name == BuildingPatternNames.Forest))));
        }



        private static void GenerateDefaultArea(Area area, RandomCollection<BuildingPattern> randomCollection)
        {
            for (var y = 0; y < World.AreaSize; y++)
            {
                for (var x = 0; x < World.AreaSize; x++)
                {
                    var pos = new IntVector(x, y);

                    area[pos] = new Building(
                        pos, Player.Nature, area,
                        randomCollection.Get());
                }
            }
        }
    }
}