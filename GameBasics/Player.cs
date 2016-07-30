using System;
using System.Collections.Generic;
using CompressedStructures;
using GameBasics.Buildings;
using GameBasics.PlayerClasses;

namespace GameBasics
{
    [Serializable]
    public class Player : IRefreshable
    {
        public class RefreshEventArgs : EventArgs
        {
            public Player Owner { get; }

            public RefreshEventArgs(Player owner)
            {
                Owner = owner;
            }
        }



        public static Player Nature { get; }
        public static Player Enemy { get; }

        public string Name { get; private set; }

        public List<Building> OwnedBuildings { get; }
        public Leader MainLeader { get; set; }
        public Resources CurrentResources { get; }

        public Territory Territory { get; }
        public World World { get; }
        public Game Game { get; }

        public event EventHandler<RefreshEventArgs> OnRefresh;



        public const int StartWood = 1000;
        public const int StartMeat = 600;
        public const int StartGold = 1000;
        // TODO to GameValues


        static Player()
        {
            Nature = new Player("nature");
            Enemy = new Player("enemy");
        }

        public Player(string name, World world, Game currentGame) : this(name)
        {
            this.Game = currentGame;
            this.World = world;
            this.Territory = World.NewPlayerTerritory(this);

            CurrentResources = new Resources() {
                Resource = {
                    // TODO enable
//                    [ResourceType.Wood] = GameValues.Instance.StartWood,
//                    [ResourceType.Meat] = GameValues.Instance.StartMeat,
//                    [ResourceType.Gold] = GameValues.Instance.StartGold,
                }
            };
        }

        private Player(string name)
        {
            Name = name;
            OwnedBuildings = new List<Building>();
        }


        public void Refresh()
        {
            MainLeader.Refresh();
            CurrentResources.Refresh();

            try
            {
                OnRefresh?.Invoke(this, new RefreshEventArgs(this));
            }
            catch
            {
                // TODO log exception output
            }
        }
    }
}

