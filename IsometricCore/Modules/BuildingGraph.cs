using System;
using IsometricCore.Structures;
using IsometricCore.Modules.WorldModule.Buildings;

namespace IsometricCore.Modules
{
    [Serializable]
    public class BuildingGraph
    {
        #region Singleton-part

        private static Graph<BuildingPattern> _instance;
        public static Graph<BuildingPattern> Instance {
            get { return _instance ?? (_instance = new Graph<BuildingPattern>(true)); }

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

