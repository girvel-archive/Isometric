using System;
using GameCore.Modules.WorldModule.Buildings;
using GameCore.Modules.TickModule;

namespace GameCore.Modules.WorldModule.Land
{
	[Serializable]
	public class Territory : IIndependentChanging
	{
		public TerritoryPattern Pattern { get; set; }

		public Building[,] BuildingGrid { get; }



        public int Seed { get; }

        public TerritoryGenerationType Type { get; }

        protected Random Random;



		public Territory() {}



        #region Interface

        void IIndependentChanging.Tick()
        {
            Pattern.Tick(this);
        }

        #endregion
	}
}

