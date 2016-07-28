using System;
using GameBasics;

namespace GameRealization
{
    [Serializable]
    public class WorldRealization : World
    {
        public WorldRealization(int seed) : base(seed) {}



        public override Territory NewPlayerTerritory(Player player)
        {
            int randx, randy;
            Territory terr;
            do
            {
                randx = BasicsHelper.MainRandom.Next(Size);
                randy = BasicsHelper.MainRandom.Next(Size);
                terr = Territories[randx, randy];
            }
            while (terr != null && terr.Type == TerritoryType.PlayerVillage);

            var newForest = new Territory(
                TerritoryPatterns.Forest, 
                TerritoryType.PlayerVillage, 
                this, 
                BasicsHelper.MainRandom.Next());
            
            newForest.Pattern.GenerateVillage(newForest, player);

            Territories[randx, randy] = newForest;
            return newForest;
        }
    }
}