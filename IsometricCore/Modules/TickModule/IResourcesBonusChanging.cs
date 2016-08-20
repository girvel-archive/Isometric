using System;
using IsometricCore.Modules.PlayerModule;
using CommonStructures;

namespace IsometricCore.Modules.TickModule
{
    public interface IResourcesBonusChanging
    {
        void Tick(ref Resources resources);
    }
}

