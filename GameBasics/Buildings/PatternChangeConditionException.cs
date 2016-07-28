using System;
using System.Runtime.Serialization;

namespace GameBasics.Buildings
{
    [Serializable]
    public class PatternChangeConditionException : Exception
    {
        public PatternChangeConditionException() {}
        public PatternChangeConditionException(string message) : base(message) {}
        public PatternChangeConditionException(string message, Exception inner) : base(message, inner) {}

        protected PatternChangeConditionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}