using System;
using VectorNet;

namespace Isometric.CommonStructures
{
    [Serializable]
    public class UpgradeResult
    {
        public short ID { get; set; }

        public IntVector Position { get; set; }



        public UpgradeResult(short id, IntVector position)
        {
            ID = id;
            Position = position;
        }

        [Obsolete("using serialization ctor")]
        public UpgradeResult() { }
    }
}