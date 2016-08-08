using System;
using GameCore.Modules.TickModule;
using GameCore.Modules.TimeModule;

namespace GameCore.Modules.PlayerModule.Leaders
{
    // TODO leader dna

	[Serializable]
	public class Leader : IIndependentChanging, IResourcesBonusChanging
	{
		public LeaderPattern Pattern { get; set; }

		public Name Name { get; set; }

		public GameDate Age { get; set; }

		public GameDate AgeMax { get; set; }



		public Leader() {}

		public Leader(LeaderPattern pattern)
		{
			Pattern = pattern;
			this.Name = NamesGenerator.Instance.Generate();

			Age = SingleRandom.Next(
				GlobalData.Instance.MinimalNewLeaderAge,
				GlobalData.Instance.MaximalNewLeaderAge);

			AgeMax = Age + SingleRandom.Next(
				GlobalData.Instance.MinimalLeaderLifeDuration,
				GlobalData.Instance.MaximalLeaderLifeDuration);
		}

		public void Die()
		{
			throw new NotImplementedException();
		}



		#region Interfaces

		void IIndependentChanging.Tick()
		{
			Age += GlobalData.Instance.DaysInTick;

            if (Age > AgeMax)
            {
                Die();
            }
		}

		#endregion
	}
}

