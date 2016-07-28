using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandInterface
{
    public static class Parcer
    {
        public static readonly char[] 
            OpeningBraces = {'(', '['},
            ClosingBraces = {')', ']'};


        
        public static string[] ParseList(this string str)
            => ParseList(str, s => s);

        public static Dictionary<string, string> ParseDictionary(this string str)
            => ParseDictionary(str, s => s, s => s);



        // [a:b|c:d]
        public static Dictionary<TKey, TValue> ParseDictionary<TKey, TValue>(
            this string str, Func<string, TKey> convertToKey, Func<string, TValue> convertToValue)
        {
            if (!Regex.IsMatch(str, @"^\[[\w\W]*\]$"))
            {
                throw new ArgumentException("wrong format");
            }
            
            str = str.Substring(1, str.Length - 2);

            var result = new Dictionary<TKey, TValue>();
            var pairs = str.ParseCustomList('|').Select(e => e.ParseCustomList(':'));

            try
            {
                foreach (var pair in pairs)
                {
                    result[convertToKey(pair[0])] = convertToValue(pair[1]);
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("value missing");
            }

            return result;
        }

        // (a;b;c)
        public static T[] ParseList<T>(this string str, Func<string, T> convertFromArgument)
        {
            if (!Regex.IsMatch(str, @"^\((\w|\W)*\)$"))
            {
                throw new ArgumentException("wrong format");
            }

            str = str.Substring(1, str.Length - 2);

            return str.ParseCustomList(';', convertFromArgument);
        }
        
        // [a;b;c]
        public static Dictionary<string, string> ParseType(this string str, string pattern)
        {
            var regex = new Regex(@"^\[(\w|\W)*\]$");
            if (!regex.IsMatch(pattern) || !regex.IsMatch(str))
            {
                throw new ArgumentException("wrong format");
            }

            var keys = pattern.Substring(1, pattern.Length - 2).ParseCustomList(';');
            var values = str.Substring(1, str.Length - 2).ParseCustomList(';');

            if (keys.Length != values.Length)
            {
                throw new ArgumentException("string does not match the pattern");
            }

            var result = new Dictionary<string, string>();
            for (var i = 0; i < keys.Length; i++)
            {
                result[keys[i]] = values[i];
            }

            return result;
        }



        internal static T[] ParseCustomList<T>(
            this string str, 
            char separator, 
            Func<string, T> convertFromArgument, 
            bool replaceIgnore = false, 
            char ignore = '\\')
        {
            var result = new List<T>();

            var currentElement = "";
            var braces = 0;
            for (var i = 0; i < str.Length; i++)
            {
                var symbol = str[i];
                var prevSymbol = i == 0 ? '\0' : str[i - 1];

                if (OpeningBraces.Contains(symbol)
                    && prevSymbol != ignore)
                {
                    braces++;
                }
                else if (ClosingBraces.Contains(symbol)
                    && prevSymbol != ignore)
                {
                    braces--;
                }

                if (symbol == separator
                    && braces == 0
                    && prevSymbol != ignore)
                {
                    result.Add(convertFromArgument(currentElement));
                    currentElement = "";
                }
                else
                {
                    if (!replaceIgnore || symbol != ignore || prevSymbol == ignore)
                        currentElement += symbol;
                }
            }
            result.Add(convertFromArgument(currentElement));

            if (braces != 0)
            {
                throw new ArgumentException("Braces error");
            }

            return result.ToArray();
        }

        internal static string[] ParseCustomList(
            this string str,
            char separator,
            bool replaceIgnore = false,
            char ignore = '\\')
            => ParseCustomList(str, separator, s => s, replaceIgnore, ignore);



        public static string ToArgument(this object obj)
            => _toArgument(obj as string ?? obj.ToString());

        public static string ToArgumentType(this IEnumerable<object> elements, bool convert = true)
        {
            return convert
                ? ToArgumentType(elements, ToArgument)
                : ToArgumentType(elements, s => s.ToString());
        }

        public static string ToArgumentList(this IEnumerable<object> elements, bool convert = true)
        {
            return convert 
                ? ToArgumentList(elements, ToArgument) 
                : ToArgumentList(elements, s => s.ToString());
        }

        public static string ToArgumentDictionary<TKey, TValue>(this Dictionary<TKey, TValue> elements, bool convert = true)
        {
            return convert
                ? ToArgumentDictionary(elements, ToArgument)
                : ToArgumentDictionary(elements, s => s.ToString());
        }



        private static string _toArgument(this string str)
        {
            return new[] { ",", ";", ":", "|", "[", "]", "(", ")" }.Aggregate(
                str, (current, reservedSymbol) => current.Replace(reservedSymbol, "\\" + reservedSymbol));
        }

        public static string ToArgumentType(this IEnumerable<object> elements, Func<object, string> convertToArgument)
        {
            return $"[{elements.ToCustomList(';', convertToArgument)}]";
        }

        public static string ToArgumentList<T>(this IEnumerable<T> elements, Func<T, string> convertToArgument)
        {
            return $"({elements.ToCustomList(';', convertToArgument)})";
        }

        public static string ToArgumentDictionary<TKey, TValue>(this Dictionary<TKey, TValue> elements, Func<object, string> convertToArgument)
        {
            return "[" + elements.Select(e => e.Key.ToArgument() + ':' + e.Value.ToArgument()).ToCustomList('|', o => o.ToString()) + "]";
        }

        internal static string ToCustomList<T>(this IEnumerable<T> elements, char separator, Func<T, string> convertToArgument)
        {
            var result = elements.Aggregate("", (current, field) =>
                current + convertToArgument(field) + separator);
            return result.Substring(0, result.Length - 1);
        }
    }
}