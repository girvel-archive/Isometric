using Isometric.Editor.Extensions;
using System;
using System.Collections.Generic;
using Isometric.CommonStructures;

namespace Isometric.Editor.Extensions
{
    public static class UniversalParser
    {
        public static bool TryParse(this string str, Type type, out object obj)
        {
            obj = null;
            bool result;
            if (type == typeof(Resources))
            {
                Resources resources;
                result = str.TryParse(out resources);
                obj = resources;
            }
            else if (type == typeof (int))
            {
                int integer;
                result = int.TryParse(str, out integer);
                obj = integer;
            }
            else
            {
                throw new NotImplementedException("type not supported");
            }

            return result;
        }
    }
}