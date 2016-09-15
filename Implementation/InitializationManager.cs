using System;
using Isometric.Implementation.Modules.DataImplementation;
using Isometric.Implementation.Modules.PatternsImplementation;

namespace Isometric.Implementation
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

