using System;

namespace CommonStructures
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

