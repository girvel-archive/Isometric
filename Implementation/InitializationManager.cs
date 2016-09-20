using System;
using System.IO;
using Isometric.Implementation.Modules.DataImplementation;
using Isometric.Implementation.Modules.GameData;

namespace Isometric.Implementation
{
    public static class InitializationManager
    {
        public static void Init(Stream stream)
        {
            GameDataManager.Instance = new GameDataManager(stream);

            TimeDataImplementation.Init();
            GlobalDataImplementation.Init();
            PlayerDataImplementation.Init();
            TickDataImplementation.Init();
            WorldDataImplementation.Init();
            GameConstantsManager.SetConstants();
        }
    }
}

