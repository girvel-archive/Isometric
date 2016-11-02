using System.IO;
using Isometric.Game.Modules;
using Isometric.GameDataTools.Exceptions;

namespace Isometric.Game
{
    public static class InitializationManager
    {
        /// <exception cref="InvalidGameDataException">Thrown when game data from stream is invalid</exception>
        public static void Init(Stream stream)
        {
            SingleDataManager.Instance = new SingleDataManager(stream);
        }
    }
}

