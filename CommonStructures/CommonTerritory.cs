using System;

namespace Isometric.CommonStructures
{
    [Serializable]
    public struct CommonTerritory
    {
        public int[,] PatternIDs { get; set; }

        public CommonTerritory(int[,] patternIDs)
        {
            PatternIDs = patternIDs;
        }
    }
}

