using System;

namespace IsometricCore.Modules
{
    [Serializable]
    public class GlobalData
    {
        public static GlobalData Instance { get; set; }



        public Action<Exception> OnUnknownException;



        public void RefreshValues()
        {
        }
    }
}

