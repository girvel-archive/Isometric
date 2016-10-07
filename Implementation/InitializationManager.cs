using System;
using System.IO;
using Isometric.GameDataTools.Exceptions;
using Isometric.Implementation.Modules.DataImplementation;
using Isometric.Implementation.Modules.GameData;

namespace Isometric.Implementation
{
    public static class InitializationManager
    {
        /// <exception cref="InvalidGameDataException">Thrown when game data from stream is invalid</exception>
        public static void Init(Stream stream)
        {
            GameDataManager.Instance = new GameDataManager(stream);

            TimeDataImplementation.Init();
            GlobalDataImplementation.Init();
            PlayerDataImplementation.Init();
            TickDataImplementation.Init();
        }
    }
}

