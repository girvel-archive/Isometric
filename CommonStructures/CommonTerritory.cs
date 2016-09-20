using System;

namespace Isometric.CommonStructures
{
    [Serializable]
    public struct CommonArea
    {
        public int[,] PatternIDs { get; set; }

        public CommonArea(int[,] patternIDs)
        {
            PatternIDs = patternIDs;
        }
    }
}

