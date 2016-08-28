using IsometricCore.Modules.TimeModule;
using CommonStructures;

namespace IsometricCore.Modules.PlayerModule
{
    public class PlayerData
    {
        public PlayerData() {}



        public Resources DefaultPlayerResources;

        public GameDate 
            MinimalNewLeaderAge,
            MaximalNewLeaderAge,
            MinimalLeaderLifeDuration,
            MaximalLeaderLifeDuration;



        public void RefreshDependentValues()
        {

        }
    }
}

