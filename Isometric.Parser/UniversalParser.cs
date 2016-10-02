using System;
using System.Collections;
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
                IntParser.Instance,
            };
        }



        public static bool TryParse(this string str, Type type, out object obj)
        {
            var parser = Parsers.FirstOrDefault(p => p.Type == type);
            if (parser != null)
            {
                return parser.TryParse(str, out obj);
            }

            throw new NotImplementedException("type not supported");
        }



        public static string GetValueString(this object obj, Type type)
        {
            var parser = Parsers.FirstOrDefault(p => p.Type == type);
            if (parser != null)
            {
                return parser.GetValueString(obj);
            }

            throw new NotImplementedException("type not supported");
        }
    }
}
