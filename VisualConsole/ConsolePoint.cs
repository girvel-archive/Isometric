using System;

namespace VisualConsole
{
    internal struct ConsolePoint : IConsolePoint
	{
        public char Character { get; }
        public ConsoleColor Color { get; }

        public ConsolePoint (char character, ConsoleColor color)
		{
            Character = character;
            Color = color;
		}
	}
}

