using System;
using System.Text;
using VectorNet;

namespace CommonStructures
{
    [Serializable]
    public struct CommonBuilding
    {
        public IntVector Position { get; set; }



        public CommonBuilding(IntVector position)
        {
            Position = position;
        }
    }
}