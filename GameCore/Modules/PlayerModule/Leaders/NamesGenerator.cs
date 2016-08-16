using System;
using EnumerableExtensions;

namespace GameCore.Modules.PlayerModule.Leaders
{
	[Serializable]
	public class NamesGenerator
	{
		#region Singleton-part

		private static NamesGenerator _instance;
		public static NamesGenerator Instance {
			get {
				if (_instance == null)
				{
					_instance = new NamesGenerator();
				}

				return _instance;
			}

			set {
#if DEBUG
				if (_instance != null)
				{
					throw new ArgumentException("LeaderNamesGenerator.Instance is already set");
				}
#endif

				_instance = value;
			}
		}

		public NamesGenerator() {}

		#endregion



		public string[] FirstNames { get; set; }

		public string[] LastNames { get; set; }



		public Name Generate() 
		{
			return new Name(
				FirstNames.GetRandom(SingleRandom.Instance),
				LastNames.GetRandom(SingleRandom.Instance));
		}
	}
}

