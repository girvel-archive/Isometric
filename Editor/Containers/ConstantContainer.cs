using System;
using Isometric.Editor.Extensions;

namespace Isometric.Editor.Containers
{
    public class ConstantContainer
    {
        private object _value;
        public object Value
        {
            get { return _value; }
            private set { _value = value; }
        }

        public Type Type { get; }



        public ConstantContainer(object value, Type type)
        {
            Value = value;
            Type = type;
        }

        public bool TrySet(string strValue)
        {
            return strValue.TryParse(Type, out _value);
        }
    }
}