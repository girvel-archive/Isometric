using System;

namespace GameBasics
{
    public abstract class GameValuesGenerator : Singleton<GameValuesGenerator>
    {
        public abstract GameValues Generate();
    }
}

