using System;
using VectorNet;

namespace Isometric.CommonStructures
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