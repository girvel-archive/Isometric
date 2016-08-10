using System;
using GameCore.Modules.TimeModule;

namespace GameCore.Modules.PlayerModule
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

