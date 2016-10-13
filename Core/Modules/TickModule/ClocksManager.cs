using System;
using System.Threading;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.SettingsModule;
using Isometric.Core.Modules.WorldModule;

namespace Isometric.Core.Modules.TickModule
{
    public class ClocksManager
    {
        public event Action OnTick;

        [GameConstant]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public static short DaysInTick { get; private set; }

        [GameConstant]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public static int TickLengthMilliseconds { get; private set; }



        public IIndependentChanging[] Subjects { get; }



        public ClocksManager(params IIndependentChanging[] subjects)
        {
            Subjects = subjects;
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
                Thread.Sleep(TickLengthMilliseconds);
            }
        }
    }
}

