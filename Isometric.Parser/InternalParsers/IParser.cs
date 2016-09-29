namespace Isometric.Parser.InternalParsers
{
    internal interface IParser
    {

    }

    internal interface IParser<T> : IParser
    {
        bool TryParse(string str, out T result);

        string GetValueString(T subject);
    }
}