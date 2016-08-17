using System;
using GameCore.Modules.PlayerModule;
using CommonStructures;

namespace GameCore.Modules.TickModule
{
    public interface IResourcesBonusChanging
    {
        void Tick(ref Resources resources);
    }
}

