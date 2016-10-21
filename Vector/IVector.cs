namespace Isometric.Vector
{
	public interface IVector<T>
	{
		T this[int dimension] { get; }

		T X { get; }

		T Y { get; }

		T Z { get; }
	}
}

