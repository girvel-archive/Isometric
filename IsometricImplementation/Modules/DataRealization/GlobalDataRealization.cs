using IsometricCore.Modules;

namespace IsometricImplementation.Modules.DataRealization
{
    internal static class GlobalDataRealization
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

