using System.Linq;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using Isometric.Implementation.Modules.PatternsImplementation;

namespace Isometric.Implementation.Modules.DataImplementation
{
    internal static class WorldDataImplementation
    {
        internal static void Init()
        {
            World.Data = new WorldData 
                {
                    TerritorySize = 32,

                    StartBuildings = new[] {
                        new WorldData.DefaultBuilding(5, BuildingPatterns.WoodHouse),
                    },

                    GenerateTerritory = 
                        (land, x, y, seed) => new Territory(TerritoryPatterns.Forest, seed),

                    NewPlayerTerritory = 
                        (owner, territory) =>
                        {
                            foreach (var building in World.Data.StartBuildings)
                            {
                                for (var i = 0; i < building.Number; i++)
                                {
                                    var randomPosition = SingleRandom.Next(World.Data.TerritoryVectorSize);

                                    if (!World.Data.StartBuildings.Any(
                                        b => b.Pattern == territory[randomPosition]?.Pattern))
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

