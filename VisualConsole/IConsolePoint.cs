using System;

namespace VisualConsole {
    /// <summary>
    /// Inheritor can be displayed as one colored <c>char</c>
    /// </summary>
    public interface IConsolePoint {
        /// <summary>
        /// <c>ConsoleColor</c> using to display object in console
        /// </summary>
        ConsoleColor Color { get; }
        /// <summary>
        /// <c>char</c> using to display object in console
        /// </summary>
        char Character { get; } 
    }
}