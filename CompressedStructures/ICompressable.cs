namespace CompressedStructures
{
    public interface ICompressable
    {
        byte[] GetBytes { get; }
        string GetString { get; }
    }
}