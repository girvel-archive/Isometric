using System;

namespace VisualConsole.Exceptions {

    [Serializable]
    public class WrongCommandException : CommandException {
        public WrongCommandException(string command) : base(command) { }
        public WrongCommandException(string message, string command) : base(message, command) { }
        public WrongCommandException(string message, Exception inner, string command) : base(message, inner, command) { }
        protected WrongCommandException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
