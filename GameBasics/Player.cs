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



        public static readonly Player Nature = new Player("nature");
        public static readonly Player Enemy = new Player("enemy");

        public string Name { get; private set; }

        public List<Building> Buildings { get; }
        public Leader MainLeader { get; set; }
        public Resources Resources { get; }

        public Territory Territory { get; }
        public World World { get; }
        public Game Game { get; }

        public event EventHandler<RefreshEventArgs> OnRefresh;



        public const int StartWood = 1000;
        public const int StartMeat = 600;
        public const int StartGold = 1000;



        public Player(string name, World world, Game currentGame) : this(name)
        {
            Game = currentGame;
            World = world;
            Territory = World.NewPlayerTerritory(this);
        }

        protected Player(string name)
        {
            Name = name;
            Buildings = new List<Building>();
            Resources = new Resources(this) {
                Resource = {
                    [ResourceType.Wood] = StartWood,
                    [ResourceType.Meat] = StartMeat,
                    [ResourceType.Gold] = StartGold,
                }
            };
        }


        public void Refresh()
        {
            MainLeader.Refresh();
            Resources.Refresh();

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

