﻿using System;
using System.Collections.Generic;
using IsometricCore.Extensions;
using IsometricCore.Modules.TickModule;
using IsometricCore.Modules.WorldModule.Land;
using IsometricCore.Modules.WorldModule.Buildings;
using IsometricCore.Modules.WorldModule;
using CommonStructures;

namespace IsometricCore.Modules.PlayerModule
{
    [Serializable]
    public class Player : IIndependentChanging
    {
        #region Data singleton

        [Obsolete("using backing field")]
        private static PlayerData _data;

        #pragma warning disable 618

        public static PlayerData Data {
            get {
                if (_data == null)
                {
                    _data = new PlayerData();
                }

                return _data;
            }

            set {
                #if DEBUG
                if (_data != null)
                {
                    throw new ArgumentException("Data is already set");
                }
                #endif

                _data = value;
            }
        }

        #pragma warning restore 618

                          #endregion



        public static Player Nature { get; }
        public static Player Enemy { get; }

        public string Name { get; set; }



        public Building[] GetOwnedBuildings() => _ownedBuildings?.ToArray();

        private List<Building> _ownedBuildings { get; }



        // TODO 1.x Player's leader

        public Resources CurrentResources { get; set; }

        public Territory Territory { get; set; }

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

            this.Territory = World.Instance.NewPlayerTerritory(this);

            CurrentResources = Data.DefaultPlayerResources;
        }



        public void Tick()
        {
            foreach (var subject in IndependentSubjects)
            {
                subject.Tick();
            }

            Resources resourcesDelta = new Resources();
            foreach (var subject in ResourceSubjects)
            {
                resourcesDelta += subject.Tick();
            }

            foreach (var subject in ResourceBonusSubjects)
            {
                subject.Tick(ref resourcesDelta);
            }

            DelegateExtensions.SafeInvoke(
                () => OnTick(this),
                GlobalData.Instance.OnUnknownException);
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
    }
}
