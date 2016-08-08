using System;
using System.Collections.Generic;
using VectorNet;
using System.Collections;

namespace GameCore.Structures
{
	[Serializable]
	public class LazyArray<T> : IEnumerable<T>, ICloneable, ICollection
	{
		public T this[IntVector position] {
			get {
				return this[position.X, position.Y];
			}

			set {
				this[position.X, position.Y] = value;
			}
		}

		public T this[int x, int y] {
			get {
				if (_array[x, y] == null)
				{
					_array[x, y] = GenerateElement(new IntVector(x, y));
				}

				return _array[x, y];
			}

			set {
				_array[x, y] = value;
			}
		}

		public Func<IntVector, T> GenerateElement { get; set; }

		private T[,] _array;



		public LazyArray(int sizeX, int sizeY, Func<IntVector, T> generateElement)
		{
			_array = new T[sizeX, sizeY];
		}

		public LazyArray(IntVector size, Func<IntVector, T> generateElement)
			: this(size.X, size.Y, generateElement) {}



		#region Interfaces 

		public int Count => _array.Count;

		public bool IsSynchronized => _array.IsSynchronized;

		public object SyncRoot => _array.SyncRoot;



		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return (IEnumerator<T>) _array.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		public object Clone()
		{
			return _array.Clone();
		}

		public void CopyTo(Array array, long index)
		{
			_array.CopyTo(array, index);
		}

		#endregion
	}
}

