using System;
using System.Runtime.Serialization;

namespace GameBasics.PlayerClasses
{
    [Serializable]
    public class ResourcesException : Exception
    {
        public ResourcesException() {}
        public ResourcesException(string message) : base(message) {}
        public ResourcesException(string message, Exception inner) : base(message, inner) {}

        protected ResourcesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}