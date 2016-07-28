using System;
using VectorNet;

namespace VisualConsole {
    public static class ConsoleUIHelper {
        private static readonly object Lock = new object();



        public static void Write(
            string line, int x, int y, ConsoleColor color, bool saveColor = true) 
        {
            lock (Lock) 
            {
                ConsoleColor oldColor = ConsoleColor.Gray;
                if (saveColor)
                {
                    oldColor = Console.ForegroundColor;
                }

                Console.ForegroundColor = color;
                Console.SetCursorPosition(x, y);
                Console.Write(line);

                if (saveColor)
                {
                    Console.ForegroundColor = oldColor;
                }
            }
        }

        public static void WriteMessage(
            string message, ConsoleColor color, bool saveColor = true)
        {
            lock (Lock) 
            {
                ConsoleColor oldColor = ConsoleColor.Gray;
                if (saveColor)
                {
                    oldColor = Console.ForegroundColor;
                }

                Console.ForegroundColor = color;
                Console.Write(message);

                if (saveColor)
                {
                    Console.ForegroundColor = oldColor;
                }
            }
        }

        public static void WriteGrid(
            IConsolePoint[,] grid, 
            IntVector begin, 
            IntVector end, 
            IntVector offset,
            IConsolePoint defaultFiller)
        {
            lock (Lock)
            {
                var oldColor = Console.ForegroundColor;

                for (var y = 0; y < grid.GetLength(1); y++)
                {
                    for (var x = 0; x < grid.GetLength(0); x++)
                    {
                        int absx = offset.X + x, absy = offset.Y + y;

                        IConsolePoint subject;
                        if (absx < 0 || absy < 0)
                        {
                            subject = defaultFiller;
                        } 
                        else 
                        {
                            subject = grid[absx, absy];
                        }

                        Console.ForegroundColor = subject.Color;
                        Console.Write(subject.Character);
                    }
                }

                Console.ForegroundColor = oldColor;
            }
        }



        public static void Write(
            string line, IntVector position, ConsoleColor color, bool saveColor = false) 
        {
            Write(line, position.X, position.Y, color, saveColor);
        }
    }
}
