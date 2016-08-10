using System;

namespace GameCore.Modules.TickModule
{
	public class ClocksManager
	{
        #region Data singleton

        [Obsolete("using backing field")]
        private static TickData _data;

        #pragma warning disable 0618

        public static TickData Data {
            get {
                if (_data == null)
                {
                    _data = new TickData();
                }

                return _data;
            }

            set {
                #if DEBUG
                if (_data != null)
                {
                    throw new ArgumentException("Data is already set");
                }
                #endif

                _data = value;
            }
        }

        #pragma warning restore 0618

        #endregion



		#region Singleton-part

		private static ClocksManager _instance;
		public static ClocksManager Instance {
			get {
				if (_instance == null)
				{
					_instance = new ClocksManager();
				}

				return _instance;
			}

			set {
				if (_instance != null)
				{
					throw new ArgumentException("ClocksManager.Instance is already set");
				}

				_instance = value;
			}
		}

		private ClocksManager() {}

		#endregion



		public IIndependentChanging[] Subjects { get; set; }



		public void Tick()
		{
			foreach (var subject in Subjects) 
			{
				subject.Tick();
			}
		}
	}
}

