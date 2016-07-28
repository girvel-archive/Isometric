using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumerableExtensions;

namespace VectorNet
{
    [Serializable]
    public struct IntVector
    {
        public int[] Coordinates;
        public int Dimensions;



        public byte[] GetBytes
            => EnumerableHelper.Add(
                Coordinates.Select(c => (IEnumerable<byte>) BitConverter.GetBytes(c)).ToArray());

        public string GetString
            => Encoding.ASCII.GetString(GetBytes);



        public int this[int dimension] => Coordinates[dimension];

        public int X
        {
            get
            {
                _check(1);
                return Coordinates[0];
            }
        }

        public int Y
        {
            get
            {
                _check(2);
                return Coordinates[1];
            }
        }

        public int Z
        {
            get
            {
                _check(3);
                return Coordinates[2];
            }
        }

        private void _check(int dimensionsMinimal)
        {
            if (Dimensions < dimensionsMinimal)
            {
                throw new IndexOutOfRangeException();
            }
        }

        public double Distance => Math.Sqrt(Coordinates.Sum(c => c * c));



        public IntVector(params int[] coordinates)
        {
            Coordinates = coordinates;
            Dimensions = Coordinates.Length;
        }

        public static IntVector GetFromBytes(byte[] bytes)
        {
            if (bytes.Length % 4 != 0)
            {
                throw new ArgumentException("argument 'bytes' has wrong length");
            }

            var coordinates = new int[bytes.Length / 4];

            for (var i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = BitConverter.ToInt32(bytes.Slice(i * 4, i * 4 + 3), 0);
            }

            return new IntVector(coordinates);
        }

        public static IntVector operator +(IntVector v1, IntVector v2)
        {
            if (v1.Dimensions != v2.Dimensions)
            {
                throw new ArgumentException("Dimensions");
            }

            var coordinates = new int[v1.Dimensions];

            for (var i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = v1[i] + v2[i];
            }

            return new IntVector(coordinates);
        }

        public static IntVector operator -(IntVector v1, IntVector v2)
        {
            if (v1.Dimensions != v2.Dimensions)
            {
                throw new ArgumentException("Dimensions");
            }

            var coordinates = new int[v1.Dimensions];

            for (var i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = v1[i] - v2[i];
            }

            return new IntVector(coordinates);
        }

        public static IntVector operator -(IntVector v)
        {
            var coordinates = new int[v.Dimensions];

            for (var i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = -v[i];
            }

            return new IntVector(coordinates);
        }
    }
}