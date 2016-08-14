using System;

namespace VisualServer.Extensions
{
    public static class ArrayExtensions
    {
        public static TNew[,] TwoDimSelect<TOld, TNew>(this TOld[,] array, Func<TOld, TNew> convert)
        {
            TNew[,] result = new TNew[array.GetLength(0), array.GetLength(1)];

            for (var x = 0; x < array.GetLength(0); x++)
            {
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    result[x, y] = convert(array[x, y]);
                }
            }

            return result;
        }
    }
}

