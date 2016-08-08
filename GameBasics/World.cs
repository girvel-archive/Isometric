using System;
using System.Linq;
using VectorNet;
using VisualConsole;

namespace GameBasics
{
    [Serializable]
    public abstract class World : IRefreshable
    {
        [Serializable]
        public class Date
        {
            [Serializable]
            public enum Season
            {
                Winter,
                Spring,
                Summer,
                Autumn,
            }




            public const int DaysInMonth = 60;
            public const int MonthsInYear = 12;



            public int Year { get; set; }
            public int Month
            {
                get
                {
                    return _month;
                }
                set
                {
                    _month = value % MonthsInYear;
                    Year += (int) Math.Floor((double) value / MonthsInYear);
                }
            }
            public int Day
            {
                get { return _day; }
                set
                {
                    _day = value % DaysInMonth;
                    Month = (int) Math.Floor((double) value / DaysInMonth);
                }
            }

            private int _month;
            private int _day;

            public int AbsoluteDay => Month * DaysInMonth + Day;

            public Season CurrentSeason
            {
                get
                {
                    if (AbsoluteDay > DaysInMonth * 1.5)
                        return Season.Winter;
                    if (AbsoluteDay > DaysInMonth * 1.5)
                        return Season.Spring;
                    if (AbsoluteDay > DaysInMonth * 1.5)
                        return Season.Summer;
                    return Season.Autumn;
                }
            }



            public Date(int year, int month, int day)
            {
                Year = year;
                Month = month;
                Day = day;
            }
        }



        public Territory this[IntVector point] => Territories[point.X, point.Y];
        protected Territory[,] Territories;

        public int Seed { get; }

        public Date DateNow { get; }
        
        public const int Size = 128;



        protected World(int seed)
        {
            Seed = seed;

            DateNow = new Date(122, 3, 56);
            Territories = new Territory[Size, Size];
        }



        public void Refresh()
        {
            foreach (var terra in Territories.Where(terra => terra != null))
            {
                terra.Refresh();
            }

            DateNow.Day += RefreshHelper.RefreshPeriodDays;
        }

        public abstract Territory NewPlayerTerritory(Player player);
    }
}