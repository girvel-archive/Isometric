using System;
using GameCore.Modules.WorldModule.Buildings;
using GameCore.Modules.TickModule;
using VectorNet;

namespace GameCore.Modules.WorldModule.Land
{
	[Serializable]
	public class Territory : IIndependentChanging
	{
		public TerritoryPattern Pattern { get; set; }

		public Building[,] BuildingGrid { get; }

        public Building this[IntVector position] 
        {
            get
            {
                return BuildingGrid[position.X, position.Y];
            }
            set
            {
                BuildingGrid[position.X, position.Y] = value;
            }
        }



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

