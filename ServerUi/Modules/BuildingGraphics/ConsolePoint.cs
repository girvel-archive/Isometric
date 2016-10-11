using System;
using VisualConsole;

namespace Isometric.Client.Modules.BuildingGraphics
{
    internal class ConsolePoint : IConsolePoint
    {
        public char Character { get; set; }
        public ConsoleColor Color { get; set; }

        public ConsolePoint(char character, ConsoleColor color)
        {
            Character = character;
            Color = color;
        }
    }
}

