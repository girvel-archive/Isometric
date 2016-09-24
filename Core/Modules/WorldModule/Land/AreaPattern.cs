using System;

namespace Isometric.Core.Modules.WorldModule.Land
{
    [Serializable]
    public class AreaPattern
    {
        public ushort Id { get; }
        private static ushort _nextId;

        public Action<Area, int> Generate { get; set; } 

        public Action<Area> Tick { get; set; }



        public AreaPattern() {}

        public AreaPattern(Action<Area, int> generate)
            : this()
        {
            Generate = generate;

            Id = _nextId++;
        }


        public override string ToString() => $"{typeof (AreaPattern).Name}; Id: {Id}";
    }
}

