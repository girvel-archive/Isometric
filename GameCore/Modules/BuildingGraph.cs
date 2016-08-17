using System;
using GameCore.Structures;
using GameCore.Modules.WorldModule.Buildings;

namespace GameCore.Modules
{
    [Serializable]
    public class BuildingGraph
    {
        #region Singleton-part

        private static Graph<BuildingPattern> _instance;
        public static Graph<BuildingPattern> Instance {
            get {
                if (_instance == null)
                {
                    _instance = new Graph<BuildingPattern>(true);
                }

                return _instance;
            }

            set {
                if (_instance != null)
                {
                    throw new ArgumentException("BuildingGraph.Instance is already set");
                }

                _instance = value;
            }
        }

        public BuildingGraph() {}

        #endregion
    }
}

