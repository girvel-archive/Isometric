using System;

namespace Isometric.Core.Modules
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

