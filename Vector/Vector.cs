using System;

namespace Isometric.Vector
{
	public struct Vector<T> : IVector<T>
    {
        public readonly T[] Coordinates;
        public readonly int Dimensions;



		public T this[int dimension] {
			get {
				return Coordinates[dimension];
			}

			set {
				Coordinates[dimension] = value;
			}
		}

        public T X
        {
            get
            {
                _check(1);
                return Coordinates[0];
            }
        }

        public T Y
        {
            get
            {
                _check(2);
                return Coordinates[1];
            }
        }

        public T Z
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



        public Vector(T[] coordinates)
        {
            Coordinates = coordinates;
            Dimensions = Coordinates.Length;
        }
    }
}
