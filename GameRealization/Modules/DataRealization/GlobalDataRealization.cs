using System;
using GameCore.Modules;
using GameCore.Modules.PlayerModule;

namespace GameRealization.Modules.DataRealization
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

