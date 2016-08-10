using System;
using System.Text;
using VectorNet;

namespace CommonStructures
{
    [Serializable]
    public class CommonBuilding
    {
        public string Name { get; set; }
        public IntVector Position { get; set; }



        public CommonBuilding(string name, IntVector position)
        {
            Name = name;
            Position = position;
        }

        [Obsolete("using serialization ctor", true)]
        public CommonBuilding() { }
    }
}