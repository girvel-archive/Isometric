using Isometric.Core.Modules.TimeModule;

namespace Isometric.Game.Modules
{
    public class SingleGameDateManager
    {
        private static GameDateManager _instance;
        public static GameDateManager Instance => _instance ?? (_instance = new GameDateManager());
    }
}