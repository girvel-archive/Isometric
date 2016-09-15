using System;
using VectorNet;

namespace Isometric.CommonStructures
{
    [Serializable]
    public class UpgradeResult
    {
        public int Id { get; set; }

        public IntVector Position { get; set; }



        public UpgradeResult(int id, IntVector position)
        {
            Id = id;
            Position = position;
        }

        [Obsolete("using serialization ctor")]
        public UpgradeResult() { }
    }
}