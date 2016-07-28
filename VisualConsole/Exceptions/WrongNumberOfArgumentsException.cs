using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualConsole.Exceptions {

    [Serializable]
    public class WrongNumberOfArgumentsException : CommandException {
        public ConsoleCommand[] NearestCommands { get; private set; }

        public WrongNumberOfArgumentsException(
            IEnumerable<ConsoleCommand> nearestCommands,
            string command)
             : base(command) {

            Fill(nearestCommands);
        }

        public WrongNumberOfArgumentsException(
            string message,
            IEnumerable<ConsoleCommand> nearestCommands,
            string command)
             : base(message, command) {

            Fill(nearestCommands);
        }

        public WrongNumberOfArgumentsException(
            string message,
            Exception inner,
            IEnumerable<ConsoleCommand> nearestCommands,
            string command)
             : base(message, inner, command) {

            Fill(nearestCommands);
        }

        protected WrongNumberOfArgumentsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        protected void Fill(IEnumerable<ConsoleCommand> list) {
            NearestCommands = new ConsoleCommand[list.Count()];

            int i = 0;
            foreach (var e in list) {
                NearestCommands[i] = e;
                i++;
            }
        }
    }
}
