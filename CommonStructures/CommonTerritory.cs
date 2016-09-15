using System;

namespace Isometric.CommonStructures
{
    [Serializable]
    public struct CommonTerritory
    {
        public short[,] PatternIDs { get; set; }

        public CommonTerritory(short[,] patternIDs)
        {
            PatternIDs = patternIDs;
        }
    }
}

