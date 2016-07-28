using System;

namespace CommandInterface {
    public class Command<T> {
        public Action<string[], T> Use { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Syntax { get; private set; }

        public int ArgsN { get; private set; }

        public Command(string name, string description, string syntax, Action<string[], T> commandUse) {
            Name = name;
            Description = description;
            Use = commandUse;
            Syntax = syntax;
            ArgsN = syntax == "" ? 0 : syntax.Split(',').Length;
        }
    }
}

