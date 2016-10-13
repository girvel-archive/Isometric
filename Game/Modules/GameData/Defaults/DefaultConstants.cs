using System;
using System.Collections.Generic;
using System.Linq;
using Isometric.CommonStructures;
using Isometric.Core.Modules;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.TimeModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;

namespace Isometric.Game.Modules.GameData.Defaults
{
    internal class DefaultConstants
    {
        private static DefaultConstants _instance;
        public static DefaultConstants Instance => _instance ?? (_instance = new DefaultConstants());

        

        public Dictionary<string, object> ConstantValues { get; }



        private DefaultConstants()
        {
            ConstantValues = new Dictionary<string, object>
            {
                [nameof(World.AreaSize)] = 32,
                [nameof(World.GenerateArea)] = new World.AreaGenerator(_generateArea),
                [nameof(World.NewPlayerVillage)] = new World.VillageGenerator(_newPlayerTerritory),
                [nameof(World.StartBuildings)] = new[]
                {
                    new World.DefaultBuilding(
                        5,
                        BuildingPatternList.Instance.First(b => b.Name == BuildingPatternNames.WoodHouse))
                },
                [nameof(Player.DefaultPlayerResources)] = new Resources(wood: 1000, gold: 100, meat: 2000),
                [nameof(ClocksManager.DaysInTick)] = (short) 60,
                [nameof(ClocksManager.TickLengthMilliseconds)] = 1000,
                [nameof(GameDate.DaysInMonth)] = (byte) 60,
                [nameof(GameDate.MonthsInYear)] = (byte) 6,
                [nameof(GameDate.DaysInWeek)] = (byte) 6,
            };
        }



        private static Area _generateArea(Area[,] landGrid, int x, int y, int seed)
        {
            return new Area(
                AreaPatternList.Instance.First(p => p.Name == AreaPatternNames.Forest), 
                seed);
        }

        private static void _newPlayerTerritory(Player owner, Area area)
        {
            const int iMax = ushort.MaxValue;
            const string iMaxErrorMessage = "Wrong generation algorythm: iteration maximum was exceeded";

            int building = 0, number = 0, i = 0;

            while (true)
            {
                if (i++ > iMax)
                {
#if DEBUG
                    throw new NotImplementedException(iMaxErrorMessage);
#else
                    ErrorReporter.Instance.ReportError(iMaxErrorMessage);
#endif
                }

                if (number >= World.StartBuildings[building].Number)
                {
                    if (building >= World.StartBuildings.Length - 1)
                    {
                        break;
                    }

                    building++;
                    number = 0;
                    continue;
                }

                var randomPosition = SingleRandom.Next(World.AreaVectorSize);

                if (World.StartBuildings.Any(b => b.Pattern == area[randomPosition].Pattern))
                {
                    continue;
                }

                area[randomPosition] = new Building(randomPosition, owner, area, World.StartBuildings[building].Pattern);
                number++;
            }
        }
    }
}