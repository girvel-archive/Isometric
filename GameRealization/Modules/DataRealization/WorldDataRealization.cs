using System;
using GameCore.Modules.WorldModule;
using GameCore.Modules.WorldModule.Land;
using GameRealization.Modules.PatternsRealization;
using GameCore.Modules;
using GameCore.Modules.WorldModule.Buildings;
using System.Linq;

namespace GameRealization.Modules.DataRealization
{
    internal static class WorldDataRealization
    {
        internal static void Init()
        {
            World.Data = new WorldData() 
                {
                    TerritorySize = 32,

                    StartBuildings = new[] {
                        new WorldData.DefaultBuilding(5, BuildingPatterns.WoodHouse),
                    },

                    GenerateTerritory = 
                        (land, x, y, seed) =>
                        {
                            return new Territory(TerritoryPatterns.Forest, seed);
                        },

                    NewPlayerTerritory = 
                        (owner, territory) =>
                        {
                            foreach (var building in World.Data.StartBuildings)
                            {
                                for (var i = 0; i < building.Number; i++)
                                {
                                    var randomPosition = SingleRandom.Next(World.Data.TerritoryVectorSize);

                                    if (!World.Data.StartBuildings.Any(b => b.Pattern == territory[randomPosition]?.Pattern))
                                    {
                                        territory[randomPosition] = new Building(
                                            randomPosition, owner, territory, building.Pattern);
                                    }
                                }
                            }
                        },
                };

            World.Data.RefreshDependentValues();
        }
    }
}

