using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameBasics.Buildings;
using VectorNet;

namespace GameBasics
{
    [Serializable]
    public class Territory : IRefreshable, IEnumerable<Building>
    {
        public Building this[IntVector position] {
            get { return Buildings[position.X, position.Y]; }
            set { Buildings[position.X, position.Y] = value; }
        }

        public Building this[int x, int y] {
            get { return Buildings[x, y]; }
            set { Buildings[x, y] = value; }
        }

        public Building[,] Buildings = new Building[Size, Size];



        public const byte Size = 32; // TODO to world

        public int SizeX => Size;
        public int SizeY => Size;

        public const int VillageHouses = 5;



        public TerritoryPattern Pattern { get; set; }


        // TODO delete
        char IConsolePoint.Character => Pattern.Character;
        ConsoleColor IConsolePoint.Color => Pattern.Color;




        public int Seed { get; set; }
        public TerritoryType Type { get; set; }
        public World ParentWorld { get; private set; }

        protected Random Random { get; }



        public Territory(
            TerritoryPattern pattern, TerritoryType type, World parent, int seed)
        {
            Pattern = pattern;
            Type = type;
            ParentWorld = parent;
            Seed = seed;
            this.Random = new Random(Seed);

            Pattern.Generate(this, Random.Next());
        }



        public void Refresh()
        {
            foreach (var building in Buildings)
            {
                building.Refresh();
            }
        }

        public Building Nearest(IntVector pos, Predicate<Building> func)
        {
            var minimalDistance = new IntVector(Size, Size).Distance;
            Building nearestBuilding = null;

            foreach (var b in Buildings)
            {
                var currentDistance = ( b.Position - pos ).Distance;

                if (!func(b) || !(currentDistance < minimalDistance)) continue;

                minimalDistance = currentDistance;
                nearestBuilding = b;
            }

            return nearestBuilding;
        }



        public IEnumerator<Building> GetEnumerator()
        {
            return Buildings.Cast<Building>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}