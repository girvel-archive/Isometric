using System;
using System.Linq;
using System.Reflection;
using Isometric.Parser.InternalParsers;

namespace Isometric.Parser
{
    public static class UniversalParser
    {
        private static readonly IParser[] Parsers;



        static UniversalParser()
        {
            Parsers = new IParser[]
            {
                ResourcesParser.Instance,
                ConvertiblesParser.Instance,
            };
        }



        public static bool TryParse<T>(this string str, out T obj)
        {
            var parser = Parsers.OfType<IParser<T>>().FirstOrDefault();
            if (parser != null) 
            {
                return parser.TryParse(str, out obj);
            }

            throw new NotImplementedException("type not supported");
        }



        public static string GetValueString<T>(this T obj)
        {
            var parser = Parsers.OfType<IParser<T>>().FirstOrDefault();
            if (parser != null)
            {
                return parser.GetValueString(obj);
            }

            throw new NotImplementedException("type not supported");
        }
    }
}
