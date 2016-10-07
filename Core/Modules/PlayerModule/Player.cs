using System;
using System.Collections.Generic;
using System.Linq;
using Isometric.CommonStructures;
using Isometric.Core.Extensions;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;

namespace Isometric.Core.Modules.PlayerModule
{
    [Serializable]
    public class Player : IIndependentChanging
    {
        public static PlayerData Data { get; set; }



        public static Player Nature { get; }
        public static Player Enemy { get; }

        public string Name { get; set; }



        public Building[] GetOwnedBuildings() => _ownedBuildings?.ToArray();

        private readonly List<Building> _ownedBuildings;



        // TODO 1.x Player's leader

        public Resources CurrentResources { get; set; }

        public Area Area { get; set; }

        public event Action<Player> OnTick;



        public List<IIndependentChanging> IndependentSubjects { get; set; }

        public List<IResourcesChanging> ResourceSubjects { get; set; }

        public List<IResourcesBonusChanging> ResourceBonusSubjects { get; set; }



        static Player()
        {
            #pragma warning disable 618

            Nature = new Player() { Name = "nature", };
            Enemy = new Player() { Name = "enemy", };

            #pragma warning restore 618
        }

        [Obsolete("using serialization ctor")]
        public Player() 
        {
            PlayersManager.Instance.Players.Add(this);

            // TODO 1.x progress subject
            // TODO 1.x leader subject
            IndependentSubjects = new List<IIndependentChanging>();
            ResourceSubjects = new List<IResourcesChanging>();
            ResourceBonusSubjects = new List<IResourcesBonusChanging>();
        }



        #pragma warning disable 618 // closing is inside

        public Player(string name) : this()
        {
            #pragma warning restore 618

            Name = name;
            _ownedBuildings = new List<Building>();

            this.Area = World.Instance.NewPlayerArea(this);

            CurrentResources = Data.DefaultPlayerResources;
        }



        public void Tick()
        {
            foreach (var subject in IndependentSubjects)
            {
                subject.Tick();
            }

            var resourcesDelta = 
                ResourceSubjects.Aggregate(
                    new Resources(), 
                    (current, subject) => current + subject.Tick());

            foreach (var subject in ResourceBonusSubjects)
            {
                subject.Tick(ref resourcesDelta);
            }

            CurrentResources += resourcesDelta;

            try
            {
                OnTick?.Invoke(this);
            }
            catch (Exception ex)
            {
                ErrorReporter.Instance.ReportError($"Unknown error during {nameof(OnTick)}");
            }
        }

        public void AddOwnedBuilding(Building building) 
        {
            _ownedBuildings.Add(building);

            IndependentSubjects.Add(building);
            ResourceSubjects.Add(building);
            ResourceBonusSubjects.Add(building);
        }

        public void RemoveOwnedBuilding(Building building)
        {
            _ownedBuildings.Remove(building);

            IndependentSubjects.Remove(building);
            ResourceSubjects.Remove(building);
            ResourceBonusSubjects.Remove(building);
        }


        public override string ToString() => $"{typeof (Player).Name}; Name: {Name}";
    }
}

