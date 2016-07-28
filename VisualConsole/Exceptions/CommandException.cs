using System;

namespace VisualConsole.Exceptions {

    [Serializable]
    public class CommandException : Exception {
        public string command { get; private set; }

        public CommandException(string command) { this.command = command; }
        public CommandException(string message, string command) : base(message) { this.command = command; }
        public CommandException(string message, Exception inner, string command) : base(message, inner) { this.command = command; }
        protected CommandException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
