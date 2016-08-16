using System;
using GameBasics;
using GameBasics.Buildings;
using VectorNet;
using VisualConsole;

namespace GameRealization
{
    [Serializable]
    public static class TerritoryPatterns
    {
        public static TerritoryPattern Forest { get; }



        static TerritoryPatterns()
        {
            Forest = new TerritoryPattern(
                'f', ConsoleColor.DarkGreen, _defaultVillageGeneration);
        }



        private static void _defaultVillageGeneration(TerritoryPattern.GenerateArgs args)
        {
            for (var i = 0; i < Territory.VillageHouses; i++)
            {
				var randx = GameRandom.Instance.Next(args.Territory.SizeX);
				var randy = GameRandom.Instance.Next(args.Territory.SizeY);

                if (args.Territory[randx, randy].Pattern != PatternsRealization.WoodHouse)
                {
                    args.Territory[randx, randy] = new Building(
                        new IntVector(randx, randy), 
                        Player.Nature, 
                        this, 
                        PatternsRealization.WoodHouse);
                }
            }
        }
    }
}