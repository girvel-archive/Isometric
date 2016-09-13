using System;
using IsometricImplementation.Modules.DataImplementation;
using IsometricImplementation.Modules.PatternsImplementation;

namespace IsometricImplementation
{
    public static class InitializationManager
    {
        private static readonly Action[] Inits =
        {
            BuildingPatterns.Init,
            BuildingGraphImplementation.Init,
            TimeDataImplementation.Init,
            GlobalDataImplementation.Init,
            PlayerDataImplementation.Init,
            TickDataImplementation.Init,
            WorldDataImplementation.Init,
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

