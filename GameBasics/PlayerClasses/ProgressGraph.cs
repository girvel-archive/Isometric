using System;
using CompressedStructures;
using GameBasics.Structures;

namespace GameBasics.PlayerClasses {
    [Serializable]
    public class ProgressGraph : IRefreshable
    {
        public Graph<ProgressElement> MainGraph { get; }

        public ProgressElement CurrentResearch {
            get { return _currentResearch; }
            set {
                if (value.Ready)
                    throw new ArgumentException("Progress element is ready");
                
                _currentResearch.Researching = false;
                value.Researching = true;
                _currentResearch = value;
            }
        }

        private ProgressElement _currentResearch;

        public Player Owner { get; }


        
        public ProgressGraph(Player owner)
        {
            Owner = owner;
            MainGraph = new Graph<ProgressElement>();
        }


        
        public void Refresh()
        {
            foreach (var node in MainGraph)
            {
                node.Value.Refresh();
            }

            if (CurrentResearch == null)
                return;

            CurrentResearch.ProgressPointsNow += 
                Owner.CurrentResources.LastIncrease[ResourceType.Progress] * RefreshHelper.RefreshPeriodDays;

            if (CurrentResearch.Ready)
            {
                CurrentResearch = null;
            }
        }
    }
}

