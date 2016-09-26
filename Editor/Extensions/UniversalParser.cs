using Isometric.Editor.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using RandomExtensions;

namespace Isometric.Editor.Extensions
{
    public static class UniversalParser
    {
        public static bool TryParse(this string str, Type type, out object obj)
        {
            obj = null;
            bool result;
            if (type == typeof (Resources))
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

            return result;
        }



        public static string GetValueString(this object obj)
        {
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