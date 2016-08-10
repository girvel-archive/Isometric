using System;
using GameCore.Modules.PlayerModule;
using GameCore.Modules.TimeModule;
using VectorNet;
using GameCore.Extensions;
using GameCore.Modules.WorldModule.Buildings;

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
                    OnUnknownException(_instance, new DelegateExtensions.ExceptionEventArgs(ex));
#endif
				}
			}
		}

        #endregion



        public EventHandler<DelegateExtensions.ExceptionEventArgs> OnUnknownException;



        public void RefreshValues()
        {
        }
	}
}

