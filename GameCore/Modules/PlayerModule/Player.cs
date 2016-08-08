using System;
using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Modules.TickModule;
using GameCore.Modules.WorldModule.Land;
using GameCore.Modules.WorldModule.Buildings;
using GameCore.Modules.WorldModule;

namespace GameCore.Modules.PlayerModule
{
	[Serializable]
	public class Player : IIndependentChanging
	{
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

		public string Name { get; private set; }

		public List<Building> OwnedBuildings { get; }
		public Leader MainLeader { get; set; }
		public Resources CurrentResources { get; }

		public Territory Territory { get; }

		public event EventHandler<RefreshEventArgs> OnRefresh;
		public event EventHandler<DelegateExtensions.ExceptionEventArgs> OnException;



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

			CurrentResources = GlobalData.Instance.DefaultPlayerResources;
		}



		public void Tick()
		{
			foreach (var subject in IndependentSubjects)
			{
				subject.Tick();
			}

			Resources resourcesDelta;
			foreach (var subject in ResourceSubjects)
			{
				resourcesDelta += subject.Tick();
			}

			foreach (var subject in ResourceBonusSubjects)
			{
				subject.Tick(ref resourcesDelta);
			}

			OnRefresh.SafeInvoke(this, new RefreshEventArgs(this), OnException);
		}
	}
}

