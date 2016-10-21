using System;
using Isometric.Vector;

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