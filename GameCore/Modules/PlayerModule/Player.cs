using System;
using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Modules.TickModule;
using GameCore.Modules.WorldModule.Land;
using GameCore.Modules.WorldModule.Buildings;
using GameCore.Modules.WorldModule;
using CommonStructures;

namespace GameCore.Modules.PlayerModule
{
	[Serializable]
	public class Player : IIndependentChanging
	{
        #region Data singleton

        [Obsolete("using backing field")]
        private static PlayerData _data;

        #pragma warning disable 618

        public static PlayerData Data {
            get {
                if (_data == null)
                {
                    _data = new PlayerData();
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

        #pragma warning restore 618

        #endregion



		public class RefreshEventArgs : EventArgs
		{
			public Player Owner { get; }

			public RefreshEventArgs(Player owner)
			{
				Owner = owner;
			}
		}



		public static Player Nature { get; }
		public static Player Enemy { get; }

        public string Name { get; set; }

		public List<Building> OwnedBuildings { get; }

        // TODO Player's leader

        public Resources CurrentResources { get; set; }

        public Territory Territory { get; set; }

		public event EventHandler<RefreshEventArgs> OnRefresh;



		public IIndependentChanging[] IndependentSubjects { get; set; }

		public IResourcesChanging[] ResourceSubjects { get; set; }

		public IResourcesBonusChanging[] ResourceBonusSubjects { get; set; }



		static Player()
		{
			Nature = new Player() { Name = "nature", };
			Enemy = new Player() { Name = "enemy", };
		}

		public Player() {}

		public Player(string name)
		{
			Name = name;
			OwnedBuildings = new List<Building>();

			this.Territory = World.Instance.NewPlayerTerritory(this);

			CurrentResources = Data.DefaultPlayerResources;
		}



		public void Tick()
		{
			foreach (var subject in IndependentSubjects)
			{
				subject.Tick();
			}

            Resources resourcesDelta = new Resources();
			foreach (var subject in ResourceSubjects)
			{
				resourcesDelta += subject.Tick();
			}

			foreach (var subject in ResourceBonusSubjects)
			{
				subject.Tick(ref resourcesDelta);
			}

            OnRefresh.SafeInvoke(this, new RefreshEventArgs(this), GlobalData.Instance.OnUnknownException);
		}
	}
}

