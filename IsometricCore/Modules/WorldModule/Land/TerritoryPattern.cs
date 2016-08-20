using System;
using IsometricCore.Modules.PlayerModule;
using IsometricCore.Modules.TickModule;

namespace IsometricCore.Modules.WorldModule.Land
{
    [Serializable]
    public class TerritoryPattern
    {
        public ushort ID { get; }
        private static ushort _nextID;

        public Action<Territory, int> Generate { get; set; } 

        public Action<Territory> Tick { get; set; }



        public TerritoryPattern() {}

        public TerritoryPattern(
            Action<Territory, int> generate)
            : this()
        {
            Generate = generate;

            ID = _nextID++;
        }
    }
}

