using System;
using IsometricCore.Modules;
using IsometricCore.Modules.PlayerModule;

namespace IsometricImplementation.Modules.DataRealization
{
    public static class GlobalDataRealization
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

