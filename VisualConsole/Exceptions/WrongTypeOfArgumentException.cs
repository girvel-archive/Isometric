using System;

namespace VisualConsole.Exceptions {

    [Serializable]
    public class WrongTypeOfArgumentException : CommandException {
        public WrongTypeOfArgumentException(string command) : base(command) { }
        public WrongTypeOfArgumentException(string message, string command) : base(message, command) { }
        public WrongTypeOfArgumentException(string message, Exception inner, string command) : base(message, inner, command) { }
        protected WrongTypeOfArgumentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
