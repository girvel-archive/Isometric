using System;

namespace Isometric.Core.Modules.WorldModule.Land
{
    [Serializable]
    public class AreaPattern
    {
        public string Name { get; set; }

        public ushort Id { get; }
        private static ushort _nextId;

        public Action<Area, int> Generate { get; set; } 

        public Action<Area> Tick { get; set; }



        [Obsolete("serialization ctor")]
        public AreaPattern() {}

        public AreaPattern(string name, Action<Area, int> generate)
#pragma warning disable 618
            : this()
#pragma warning restore 618
        {
            Name = name;
            Generate = generate;

            Id = _nextId++;
        }


        public override string ToString() => $"{typeof (AreaPattern).Name}; Id: {Id}";
    }
}

