using System;
using System.Runtime.Serialization;

namespace Isometric.Implementation.Modules.GameData.Exceptions
{
    [Serializable]
    public class InvalidGameDataException : Exception
    {
        public InvalidGameDataException()
        {
        }

        public InvalidGameDataException(string message) : base(message)
        {
        }

        public InvalidGameDataException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidGameDataException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}