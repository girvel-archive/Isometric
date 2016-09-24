using System;
using System.Linq;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.SettingsModule;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule.Land;
using VectorNet;

namespace Isometric.Core.Modules.WorldModule
{
    [Serializable]
    public class World : IIndependentChanging
    {
        [NonSerialized]
        public static WorldData Data;

        [GameConstant]
        private static int AreaSize { get; set; }



        #region Singleton-part

        [NonSerialized]
        private static World _instance;
        public static World Instance 
        {
            get { return _instance ?? (_instance = new World(SingleRandom.Instance.Next())); }

            set
            {
                #if DEBUG

                if (_instance != null)
                {
                    throw new ArgumentException("Instance is already set");
                }

                #endif

                _instance = value;
            }
        }

        #endregion



        public World(int seed) 
        {
            LandGrid = new Area[Data.AreaSize, Data.AreaSize];

            Seed = seed;
            Random = new Random(Seed);
        }



        public Area[,] LandGrid { get; }

        public int Seed { get; }

        protected Random Random;



        public void CheckVersion()
        {

        }

        public Area LazyGetArea(IntVector position) => LazyGetArea(position.X, position.Y);

        public Area LazyGetArea(int x, int y)
        {
            return LandGrid[x, y] ?? (LandGrid[x, y] = Data.GenerateArea(LandGrid, x, y, SeedForPosition(x, y)));
        }

        public Area NewPlayerArea(Player player)
        {
            Area result;
            do
            {
                result = LazyGetArea(SingleRandom.Next(Data.AreaVectorSize));
            }
            while (result.Type != AreaGenerationType.Wild);

            Data.NewPlayerArea(player, result);
            result.Type = AreaGenerationType.Wild;

            return result;
        }



        public void Tick()
        {
            foreach (var territory in LandGrid.Cast<IIndependentChanging>().Where(territory => territory != null))
            {
                territory.Tick();
            }
        }


        // TODO F test it
        protected int SeedForPosition(int x, int y)
        {
            return (int)((decimal)Seed * (x * Data.AreaSize + y) / (decimal)Math.Pow(Data.AreaSize, 2));
        }


        public override string ToString() => $"{typeof (World).Name}; Seed: {Seed}";
    }
}

