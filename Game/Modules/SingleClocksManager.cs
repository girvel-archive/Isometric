using Isometric.Core.Modules.TickModule;

namespace Isometric.Game.Modules
{
    public class SingleClocksManager
    {
        private static ClocksManager _instance;
        public static ClocksManager Instance => _instance ?? (_instance = _getClocksManager());



        private static ClocksManager _getClocksManager()
        {
            return new ClocksManager(SingleWorld.Instance);
        }
    }
}