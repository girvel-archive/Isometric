using System;
using IsometricImplementation.Modules.DataRealization;

namespace IsometricImplementation
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

