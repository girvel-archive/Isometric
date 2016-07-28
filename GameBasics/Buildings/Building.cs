using System;
using System.Collections.Generic;
using System.Linq;
using CompressedStructures;
using GameBasics.PlayerClasses;
using VectorNet;
using VisualConsole;

namespace GameBasics.Buildings {
    [Serializable]
    public class Building : IConsolePoint, IRefreshable
    {
        public BuildingPattern Pattern { get; set; }
        
        public IntVector Position { get; }
        public Player Owner { get; set; }
        public Territory CurrentTerritory { get; private set; }

        public int PeopleNow { get; set; }

        public Dictionary<ResourceType, int> Resources { get; set; }

        ConsoleColor IConsolePoint.Color => Pattern.Color;
        char IConsolePoint.Symbol => Pattern.Symbol;



        public Building(IntVector position, Player owner, Territory territory, BuildingPattern pattern)
        {
            Position = position;
            Owner = owner;
            CurrentTerritory = territory;
            Pattern = pattern;

            InitFromPattern(pattern);

            if (!Owner?.Buildings?.Contains(this) ?? false)
            {
                Owner.Buildings.Add(this);
            }
        }

        ~Building()
        {
            if (Owner?.Buildings?.Contains(this) ?? false)
            {
                Owner.Buildings.Remove(this);
            }
        }



        public void Refresh()
        {
            Pattern.RefreshAction?.Invoke(this);
        }

        public void Upgrade(BuildingPattern target)
        {
            var foundedObjects = Owner.Game.BuildingGraph.Find(Pattern);

            if (foundedObjects.Length > 1)
            { 
                // TODO replace by bool field
                throw new Exception("building graph can't contain one object twice");
            }

            if (!foundedObjects[0].IsParentOf(target))
            {
                throw new ArgumentException("target is not a children of current pattern in building graph");
            }

            if (!Pattern.ChangeCondition?.Invoke(Pattern, this) ?? false)
            {
                throw new PatternChangeConditionException();
            }

            if (Pattern.NeedResources.Any(resourcePair => Owner.Resources.Resource[resourcePair.Key] < resourcePair.Value))
            {
                throw new ResourcesException();
            }

            foreach (var resourcePair in Pattern.NeedResources)
            {
                Owner.Resources.Resource[resourcePair.Key] -= resourcePair.Value;
            }

            InitFromPattern(target);
        }



        protected void InitFromPattern(BuildingPattern pattern)
        {
            Resources = new Dictionary<ResourceType, int>(Pattern.Resources);
        }
    }
}