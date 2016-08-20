using System;
using System.Collections.Generic;
using System.Threading;

namespace IsometricCore.Modules
{
    public class ActionsProcessor
    {
        #region Singleton-part

        [Obsolete("using backing field")]
        private static ActionsProcessor _instance;

        #pragma warning disable 618

        public static ActionsProcessor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionsProcessor();
                }

                return _instance;
            }

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

        #pragma warning restore 618

        #endregion



        public Thread Thread { get; private set; }

        public int DelayMilliseconds { get; set; }

        private List<Func<bool>> _actions;



        public void AddFunc(Func<bool> func)
        {
            _actions.Add(func);
        }

        public void Start()
        {
            while (true)
            {
                Step();
                Thread.Sleep(DelayMilliseconds);
            }
        }

        public void StartThread()
        {
            Thread = new Thread(Start);
            Thread.Start();
        }

        internal void Step()
        {
            foreach (var action in _actions)
            {
                if (action())
                {
                    _actions.Remove(action);
                }
            }
        }
    }
}

