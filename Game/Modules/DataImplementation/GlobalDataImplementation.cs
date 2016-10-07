using Isometric.Core.Modules;

namespace Isometric.Game.Modules.DataImplementation
{
    internal static class GlobalDataImplementation
    {
        internal static void Init()
        {
            GlobalData.Instance = new GlobalData 
            {

            };

            GlobalData.Instance.RefreshValues();
        }
    }
}

