using System;

namespace GameBasics
{
    [Serializable]
    public class TerritoryPattern
    {
        public char Symbol { get; set; }
        public ConsoleColor Color { get; set; }

        public Action<Territory, int> Generate { get; set; } 
        public Action<Territory, Player> GenerateVillage { get; set; }
        public Action<Territory> Refresh { get; set; }



        private TerritoryPattern()
        {
            Refresh = territory =>
            {
                foreach (var building in territory)
                {
                    building.Refresh();
                }
            };
        }

        public TerritoryPattern(
            char symbol, 
            ConsoleColor color, 
            Action<Territory, int> generate, 
            Action<Territory, Player> generateVillage)
            : this()
        {
            Symbol = symbol;
            Color = color;
            Generate = generate;
            GenerateVillage = generateVillage;
        }
    }
}

