using System.IO;
using Isometric.Game.Modules.DataImplementation;
using Isometric.Game.Modules.GameData;
using Isometric.GameDataTools.Exceptions;

namespace Isometric.Game
{
    public static class InitializationManager
    {
        /// <exception cref="InvalidGameDataException">Thrown when game data from stream is invalid</exception>
        public static void Init(Stream stream)
        {
            GameDataManager.Instance = new GameDataManager(stream);

            TimeDataImplementation.Init();
            PlayerDataImplementation.Init();
            TickDataImplementation.Init();
        }
    }
}

