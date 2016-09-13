using System;
using System.Threading;
using IsometricCore.Modules.PlayerModule;
using IsometricCore.Modules.WorldModule;

namespace IsometricCore.Modules.TickModule
{
    public class ClocksManager
    {
        public static TickData Data { get; set; }



        #region Singleton-part

        private static ClocksManager _instance;
        public static ClocksManager Instance {
            get { return _instance ?? (_instance = new ClocksManager()); }

            set {
                #if DEBUG

                if (_instance != null)
                {
                    throw new ArgumentException("ClocksManager.Instance is already set");
                }

                #endif

                _instance = value;
            }
        }

        #endregion



        public static event Action OnTick;



        public IIndependentChanging[] Subjects { get; }



        public ClocksManager()
        {
            Subjects = new IIndependentChanging[]
            {
                PlayersManager.Instance,
                World.Instance,
            };
        }



        public void Tick()
        {
            foreach (var subject in Subjects) 
            {
                subject.Tick();
            }
        }

        public void TickLoop()
        {
            while (true)
            {
                Tick();
                OnTick?.Invoke();
                Thread.Sleep(Data.TickLengthMilliseconds);
            }
        }
    }
}

