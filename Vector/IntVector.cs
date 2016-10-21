using System;
using System.Linq;
using Newtonsoft.Json;

namespace Isometric.Vector
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
	public struct IntVector : IVector<int>
    {
        public int[] Coordinates;
        public int Dimensions;



		public int this[int dimension] {
			get {
				return Coordinates[dimension];
			}

			set {
				Coordinates[dimension] = value;
			}
		}
        
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