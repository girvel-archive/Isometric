using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandInterface
{
    public class Interface<T>
    {
        public List<Command<T>> Commands { get; set; }



        public Interface(List<Command<T>> commands)
        {
            Commands = commands;
        }



        public void UseCommand(string command, T additionalData)
        {
            var commandParts = command.Split('@');
            string[] args = { };

            if (commandParts.Length > 1)
            {
                args = commandParts[1].ParseCustomList(',', true);
            }

            var commands =
                from cmd in Commands
                where cmd.Name == commandParts[0]
                    && cmd.ArgsN == args.Length
                select cmd;

            if (commands.Any())
            {
                commands.First().Use(args, additionalData);
            }
            else
            {
                throw new ArgumentException("unknown command");
            }
        }
    }
}
