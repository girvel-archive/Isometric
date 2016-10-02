namespace Isometric.Parser.InternalParsers
{
    internal interface IAbstractParser { }

    internal interface IAbstractParser<T> : IAbstractParser
    {
        bool TryParse<TElement>(string str, out T result, IParser<TElement> elementParser);

        string GetValueString<TElement>(T subject, IParser<TElement> elementParser);
    }
}