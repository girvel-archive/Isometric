using System;

namespace GameBasics.PlayerClasses
{
    [Serializable]
    public class Leader : IRefreshable
    {
        private static class NameHelper {
            private static string[] Names => new[] {
                "Alfhild",
                "Alva",
                "Asmund",
                "Bjorn",
                "Brandt",
                "Erika",
                "Felman",
                "Iona",
                "Mark"
            };

            public static string RandomName => Names[GameRandom.Instance.Next(Names.Length)];
        }



        public string Name { get; private set; }
        public float AgeYears { get; private set; }
        public float AgeYearsMax { get; private set; }

        public Player Owner { get; private set; }

        public bool Dead { get; private set; }



        public Leader(Player owner)
        {
            Owner = owner;
            Name = NameHelper.RandomName;
            AgeYears = GameRandom.Instance.Next(20) + 15;
        }



        public virtual void Refresh()
        {
            AgeYears += RefreshHelper.RefreshPeriodDays / RefreshHelper.DaysInYear;

            if (AgeYears > AgeYearsMax)
            {
                Owner.MainLeader = null;
                Dead = true;
                return;
            }
        }
    }
}

