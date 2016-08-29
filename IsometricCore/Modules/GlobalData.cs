using System;

namespace IsometricCore.Modules
{
    [Serializable]
    public class GlobalData
    {
        #region Singleton-part

        private static GlobalData _instance;
        public static GlobalData Instance {
            get { return _instance ?? (_instance = new GlobalData()); }
            set {
                if (_instance == null)
                {
                    _instance = value;
                }
                else
                {
                    var ex = new ArgumentException("GlobalData.Instance is already set");
#if DEBUG
                    throw ex;
#else
                    Instance.OnUnknownException?.Invoke(ex);
#endif
                }
            }
        }

        #endregion



        public Action<Exception> OnUnknownException;



        public void RefreshValues()
        {
        }
    }
}

