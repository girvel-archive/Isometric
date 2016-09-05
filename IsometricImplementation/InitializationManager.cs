using System;
using IsometricImplementation.Modules.DataRealization;
using IsometricImplementation.Modules.PatternsRealization;

namespace IsometricImplementation
{
    public static class InitializationManager
    {
        private static readonly Action[] Inits =
        {
            BuildingPatterns.Init,
            BuildingGraphRealization.Init,
            TimeDataRealization.Init,
            GlobalDataRealization.Init,
            PlayerDataRealization.Init,
            TickDataRealization.Init,
            WorldDataRealization.Init,
        };

        public static void Init()
        {
            foreach (var init in Inits)
            {
                init();
            }
        }
    }
}

