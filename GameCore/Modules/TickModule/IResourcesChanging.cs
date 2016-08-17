using System;
using GameCore.Modules.PlayerModule;
using CommonStructures;

namespace GameCore.Modules.TickModule
{
    public interface IResourcesChanging
    {
        Resources Tick();
    }
}

