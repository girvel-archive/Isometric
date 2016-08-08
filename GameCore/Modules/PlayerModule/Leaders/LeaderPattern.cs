using System;

namespace GameCore.Modules.PlayerModule.Leaders
{
	[Serializable]
	public class LeaderPattern
	{
		public Action<Leader> OnRefresh { get; set; }

		public LeaderPattern() {}

		public LeaderPattern(Action<Leader> onRefresh)
		{
			OnRefresh = onRefresh;
		}
	}
}

