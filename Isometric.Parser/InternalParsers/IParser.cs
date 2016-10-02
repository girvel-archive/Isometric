using System;

namespace Isometric.Parser.InternalParsers
{
    internal interface IParser
    {
        Type Type { get; }

        bool TryParse(string str, out object result);

        string GetValueString(object subject);
    }
}