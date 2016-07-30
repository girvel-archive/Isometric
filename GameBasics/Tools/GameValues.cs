using System;
using SingleClass;

namespace GameBasics
{
    public abstract class GameValues
	{
        private static GameValues _instance;
        public static GameValues Instance {
            get {
                if (_instance == null)
                {
                    _instance = GameValuesGenerator.Instance.Generate();
                }

                return _instance;
            }
        }



        public abstract int StartWood { get; }

        public abstract int StartMeat { get; }

        public abstract int StartGold { get; }
	}
}

