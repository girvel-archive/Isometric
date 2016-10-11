using System;

namespace Isometric.Client.Tools
{
    public class ConsoleDecorator
    {
        public static string GetLine(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public static string GetPassword(string message)
        {
            Console.Write(message);
            var result = "";

            while (true)
            {
                var c = Console.ReadKey(true);

                switch (c.Key)
                {
                    default:
                        result += c.KeyChar;
                        break;

                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return result;

                    case ConsoleKey.Backspace:
                        if (result.Length != 0)
                        {
                            result = result.Substring(0, result.Length - 1);
                        }
                        break;
                }
            }
        }
    }
}

