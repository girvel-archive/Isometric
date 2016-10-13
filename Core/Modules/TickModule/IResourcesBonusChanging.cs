using Isometric.CommonStructures;

namespace Isometric.Core.Modules.TickModule
{
    public interface IResourcesBonusChanging
    {
        void Tick(ref Resources resources);
    }
}

