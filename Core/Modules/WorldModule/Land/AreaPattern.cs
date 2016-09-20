using System;

namespace Isometric.Core.Modules.WorldModule.Land
{
    [Serializable]
    public class AreaPattern
    {
        public ushort ID { get; }
        private static ushort _nextID;

        public Action<Area, int> Generate { get; set; } 

        public Action<Area> Tick { get; set; }



        public AreaPattern() {}

        public AreaPattern(
            Action<Area, int> generate)
            : this()
        {
            Generate = generate;

            ID = _nextID++;
        }
    }
}

