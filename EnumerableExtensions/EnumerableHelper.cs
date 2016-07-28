using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static class EnumerableHelper
    {
        public static T[] Add<T>(params IEnumerable<T>[] arrays)
        {
            var result = new T[arrays.Sum(a => a.Count())];
            var i = 0;

            foreach (var e in arrays.SelectMany(array => array))
            {
                result[i++] = e;
            }

            return result;
        }

        public static T[] Slice<T>(this T[] array, int from, int to)
        {
            if (from < 0 || from >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(from));
            }

            if (to < 0 || to >= array.Length || from > to)
            {
                throw new ArgumentOutOfRangeException(nameof(to));
            }

            var result = new T[to - from + 1];

            for (var i = from; i < to; i++)
            {
                result[i - from] = array[i];
            }

            return result;
        }



        public static T[] Slice<T>(this T[] array, int from) => Slice(array, from, array.Length);
    }
}