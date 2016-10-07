using Isometric.CommonStructures;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.TimeModule;

namespace Isometric.Game.Modules.DataImplementation
{
    public static class PlayerDataImplementation
    {
        internal static void Init()
        {
            Player.Data = new PlayerData() 
                {
                    MinimalNewLeaderAge = new GameDate(15, 0, 0),
                    MaximalNewLeaderAge = new GameDate(33, 0, 0),
                    MinimalLeaderLifeDuration = new GameDate(45, 0, 0),
                    MaximalLeaderLifeDuration = new GameDate(55, 0, 0),

                    DefaultPlayerResources = new Resources(gold: 999999, meat: 999999, wood: 999999, stone: 999999),
                    // TODO change start resources
                };

            Player.Data.RefreshDependentValues();
        }
    }
}

