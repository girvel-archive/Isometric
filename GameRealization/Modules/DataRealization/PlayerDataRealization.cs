using System;
using GameCore.Modules.PlayerModule;

namespace GameRealization.Modules.DataRealization
{
    public static class PlayerDataRealization
	{
        static PlayerDataRealization ()
		{
            PlayerData.Instance = new PlayerData() 
            {
                MinimalNewLeaderAge = 15,
                MaximalNewLeaderAge = 33,
                MinimalLeaderLifeDuration = 40,
                MaximalLeaderLifeDuration = 55,

                DefaultPlayerResources = new Resources(gold: 1000, meat: 1000, wood: 1000),
            };

            PlayerData.Instance.RefreshDependentValues();
		}
	}
}

