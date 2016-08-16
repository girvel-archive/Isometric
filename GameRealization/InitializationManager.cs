using System;
using GameRealization.Modules.DataRealization;

namespace GameRealization
{
    public class InitializationManager
    {
        public static void Init()
        {
            TimeDataRealization.Init();
            GlobalDataRealization.Init();
            PlayerDataRealization.Init();
            TickDataRealization.Init();
            WorldDataRealization.Init();
        }
    }
}

