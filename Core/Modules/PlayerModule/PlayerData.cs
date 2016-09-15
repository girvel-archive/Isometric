using Isometric.CommonStructures;
using Isometric.Core.Modules.TimeModule;

namespace Isometric.Core.Modules.PlayerModule
{
    public class PlayerData
    {
        public PlayerData() {}



        public Resources DefaultPlayerResources;

        public GameDate 
            MinimalNewLeaderAge,
            MaximalNewLeaderAge,
            MinimalLeaderLifeDuration,
            MaximalLeaderLifeDuration;



        public void RefreshDependentValues()
        {

        }
    }
}

