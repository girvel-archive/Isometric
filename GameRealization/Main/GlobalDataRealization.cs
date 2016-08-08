using System;
using GameCore.Modules;
using GameCore.Modules.PlayerModule;

namespace GameRealization.Main
{
    public static class GlobalDataRealization
	{
        static GlobalDataRealization()
        {
            GlobalData.Instance = new GlobalData {
                DaysInMonth = 60,
                MonthsInYear = 6,
                DaysInWeek = 6,
                DaysInSeason = 90,
                DaysInTick = 60,

                DefaultPlayerResources = new Resources(gold: 1000, meat: 1000, wood: 1000),
                TerritorySize = 32,

                MinimalNewLeaderAge = 15,
                MaximalNewLeaderAge = 33,
                MinimalLeaderLifeDuration = 40,
                MaximalLeaderLifeDuration = 55,
            };

            GlobalData.Instance.RefreshValues();
        }
	}
}

