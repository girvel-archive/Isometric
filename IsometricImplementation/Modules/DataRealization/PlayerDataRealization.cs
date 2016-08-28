using IsometricCore.Modules.PlayerModule;
using CommonStructures;
using IsometricCore.Modules.TimeModule;

namespace IsometricImplementation.Modules.DataRealization
{
    public static class PlayerDataRealization
    {
        internal static void Init()
        {
            Player.Data = new PlayerData() 
                {
                    MinimalNewLeaderAge = new GameDate(15, 0, 0),
                    MaximalNewLeaderAge = new GameDate(33, 0, 0),
                    MinimalLeaderLifeDuration = new GameDate(45, 0, 0),
                    MaximalLeaderLifeDuration = new GameDate(55, 0, 0),

                    DefaultPlayerResources = new Resources(gold: 1000, meat: 1000, wood: 1000),
                };

            Player.Data.RefreshDependentValues();
        }
    }
}

