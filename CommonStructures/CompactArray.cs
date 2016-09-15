using System;
using System.Runtime.Serialization;

namespace Isometric.CommonStructures
{
    [Serializable]
    public struct CompactArray<T> : ISerializable
    {
        public T[] Value;

        private const string ArraySizeName = "s";




        public CompactArray(T[] value)
        {
            Value = value;
        }

        public CompactArray(SerializationInfo info, StreamingContext context)
        {
            Value = new T[info.GetInt32(ArraySizeName)];

            for (var i = 0; i < Value.Length; i++)
            {
                Value[i] = (T) info.GetValue(i.ToString(), typeof(T));
            }
        } 



        public static implicit operator T[](CompactArray<T> compact) 
            => compact.Value;

        public static implicit operator CompactArray<T>(T[] array) 
            => new CompactArray<T>(array);


        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("s", Value.Length);

            for (var i = 0; i < Value.Length; i++)
            {
                info.AddValue(i.ToString(), Value[i], typeof(T));
            }
        }
    }
}