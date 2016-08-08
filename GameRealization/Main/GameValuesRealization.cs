using System;
using GameBasics;

namespace GameRealization.Main
{
    public static class GameValuesRealization
	{
        static GameValuesRealization()
        {
            GameValues.Instance = new GameValues {
                StartWood = 1000,
                StartMeat = 1000,
                StartGold = 1000,
            };
        }
	}
}

