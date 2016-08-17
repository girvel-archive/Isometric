using System;
using GameCore.Extensions;

namespace GameCore.Modules
{
    [Serializable]
    public class GlobalData
    {
        #region Singleton-part

        private static GlobalData _instance;
        public static GlobalData Instance {
            get {
                if (_instance == null)
                {
                    _instance = new GlobalData();
                }

                return _instance;
            }
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

