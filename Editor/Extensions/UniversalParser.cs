using System;
using System.Globalization;
using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule.Buildings;
using RandomExtensions;

namespace Isometric.Editor.Extensions
{
    public static class UniversalParser
    {
        private static bool TryParse(this string str, Type type, out object obj)
        {
            throw new NotImplementedException("method has to be deleted");

            obj = null;
            bool result;
            if (type == typeof (Resources))
            {
                Resources resources;
                //result = str.TryParse(out resources);
                obj = resources;
            }
            else if (type == typeof (int))
            {
                int integer;
                result = int.TryParse(str, out integer);
                obj = integer;
            }
            else if (type == typeof (RandomCollection<BuildingPattern>))
            {
                RandomCollection<BuildingPattern> collection;
                result = str.TryParse(out collection);
                obj = collection;
            }
            else
            {
                throw new NotImplementedException("type not supported");
            }

            return obj != null && result;
        }



        public static string GetValueString(this object obj)
        {
            throw new NotImplementedException("method has to be deleted");

            {
                var convertible = obj as IConvertible;
                if (convertible != null)
                {
                    return convertible.ToString(CultureInfo.InvariantCulture);
                }
            }

            if (obj is Resources)
            {
                return ((Resources) obj).GetValueString();
            }

            throw new NotImplementedException();
        }
    }
}