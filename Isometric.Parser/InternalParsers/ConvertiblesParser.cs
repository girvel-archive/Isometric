using System;
using System.Globalization;
using System.Linq;

namespace Isometric.Parser.InternalParsers
{
    internal class ConvertiblesParser : IParser<int>
    {
        private static ConvertiblesParser _instance;
        public static ConvertiblesParser Instance = _instance ?? (_instance = new ConvertiblesParser());



        private delegate bool TryParseMethod<T>(string str, out T result);



        private ConvertiblesParser() { }



        public bool TryParse<T>(string str, out T result)
        {
            return ((TryParseMethod<T>)
                typeof (T) 
                    .GetMethods()
                    .First(method => method.Name == "TryParse" && method.GetParameters().Length == 2)
                    .CreateDelegate(typeof (TryParseMethod<T>)))(str, out result);
        }

        public string GetValueString(IConvertible obj) => obj.ToString(CultureInfo.CurrentCulture);



        bool IParser<int>.TryParse(string str, out int result) => TryParse(str, out result);

        string IParser<int>.GetValueString(int obj) => GetValueString(obj);
    }
}