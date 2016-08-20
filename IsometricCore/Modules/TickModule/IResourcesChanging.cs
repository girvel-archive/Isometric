using System;
using IsometricCore.Modules.PlayerModule;
using CommonStructures;

namespace IsometricCore.Modules.TickModule
{
    public interface IResourcesChanging
    {
        Resources Tick();
    }
}

