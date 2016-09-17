using System;
using System.IO;
using Isometric.Implementation.Modules.DataImplementation;
using Isometric.Implementation.Modules.GameData;

namespace Isometric.Implementation
{
    public static class InitializationManager
    {
        private static readonly Action[] Inits =
        {
            TimeDataImplementation.Init,
            GlobalDataImplementation.Init,
            PlayerDataImplementation.Init,
            TickDataImplementation.Init,
            WorldDataImplementation.Init,
        };

        public static void Init(Stream stream)
        {
            foreach (var init in Inits)
            {
                init();
            }

            GameDataManager.Instance = new GameDataManager(stream);
        }
    }
}

