using System;

namespace GameCore.Modules.TickModule
{
	public class ClocksManager
	{
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

