using Isometric.Core.Modules.PlayerModule;

namespace Isometric.Game.Modules
{
    public static class SinglePlayersManager
    {
        private static PlayersManager _instance;
        public static PlayersManager Instance => _instance ?? (_instance = new PlayersManager());
    }
}