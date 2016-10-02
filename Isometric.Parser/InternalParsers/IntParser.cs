using System;
using System.Globalization;
using System.Linq;

namespace Isometric.Parser.InternalParsers
{
    internal class IntParser : IParser
    {
        private static IntParser _instance;
        public static IntParser Instance = _instance ?? (_instance = new IntParser());



        public Type Type => typeof (int);



        private IntParser() { }



        public bool TryParse(string str, out object obj)
        {
            int result;
            var success = int.TryParse(str, out result);
            obj = result;
            return success;
        }

        public string GetValueString(object obj) => ((int)obj).ToString();
    }
}