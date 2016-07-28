using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VectorNet;

namespace VisualConsole
{
    [Serializable]
    public class OutputDiv {
        public IntVector Begin { get; set; }
        public IntVector End { get; set; }

        public ConsoleIface Interface { get; set; }

        public int CurrentPage { get; set; }

        protected bool Reading;
        protected object WritingLock = new object();

        protected List<string> Output = new List<string>();

        private const string Separator = "~";
        
        protected string CurrentLine {
            get {
                if (Output.Last().Length >= Prefix.Length
                    && Output.Last().Substring(0, Prefix.Length) == Prefix) {
                    return Output.Last().Substring(Prefix.Length, Output.Last().Length - Prefix.Length);
                }
                return Output.Last();
            }
            set { Output[Output.Count - 1] = Prefix + value; }
        }

        public string Prefix { get; set; }

        private void variablesInitialization() {
            CurrentPage = 0;
        }

        public OutputDiv(IntVector begin, IntVector end, ConsoleIface iface, string prefix = " $ ") {
            variablesInitialization();

            Begin = begin;
            End = end;
            Interface = iface;
            Prefix = prefix;
        }

        public OutputDiv(int beginX, int beginY, int endX, int endY, ConsoleIface iface, string prefix = " $ ")
            : this(new IntVector(beginX, beginY), new IntVector(endX, endY), iface, prefix) {}
        
        public void Write(params string[] str) {
            lock (WritingLock) {
                if (str.Length < 1)
                    throw new ArgumentException();

                Output[Output.Count - 1] += str[0];

                if (str.Length > 1) {
                    var lines = new string[str.Length - 1];
                    str.CopyTo(lines, 1);
                    WriteLine(lines);
                }

                Refresh();
            }
        }

        public void WriteLine(params string[] str) {
            lock (WritingLock) {
                try {
                    if (Reading) {
                        Output.InsertRange(Output.Count - 1, str);
                    }
                    else {
                        Output.AddRange(str);
                    }
                    if (str.Length > 0)
                        Output.Add(Separator);
                    Output.Add("");

                    Refresh();
                }
                catch (Exception e) {
                    #if !UNITY
                    Console.WriteLine(e.Message);
                    #endif
                }
            }
        }

        public string ReadLine() {
            Reading = true;
            if (Output.Last() == Separator) {
                Output.Add("");
                Refresh();
            }

            ConsoleKeyInfo keyInfo;
            ConsoleKeyInfo prevKeyInfo = new ConsoleKeyInfo('O', ConsoleKey.O, true, false, false);
            do {
                ClearLine(End.Y - Begin.Y - 1);
                ConsoleWriteHelper.Write(Prefix + CurrentLine, Begin.X, End.Y - 1, ConsoleColor.Gray);
                keyInfo = Console.ReadKey();

                switch (keyInfo.Key) {
                    case ConsoleKey.Enter:
                        break;

                    case ConsoleKey.Backspace:
                        if (Console.CursorLeft - Begin.X > Prefix.Length - 1
                                || ((Output.Last().Length < Prefix.Length
                                || Output.Last().Substring(0, Prefix.Length) != Prefix)
                                && Console.CursorLeft - Begin.X >= 0))
                            try {
                                DeleteSymbols();
                            }
                            catch (ArgumentOutOfRangeException) {}
                        else
                            Console.CursorLeft++;
                        break;

                    case ConsoleKey.Tab:
                        var cmds =
                            from cmd in Interface.StringCommands
                            where cmd.Names.Any(s => s.Contains(CurrentLine))
                            select cmd;

                        if (cmds.Count() == 1)
                            CurrentLine = cmds.First().Names.First();
                        else if (cmds.Count() > 1 && prevKeyInfo.Key == ConsoleKey.Tab)
                            foreach (var consoleCommand in cmds) {
                                WriteLine(consoleCommand.FormatInfo);
                            }
                        break;

                    case ConsoleKey.Escape:
                        return "";

                    case ConsoleKey.PageDown:
                        CurrentPage = Math.Max(0, CurrentPage - 1);
                        Refresh();
                        break;

                    case ConsoleKey.PageUp:
                        CurrentPage = Math.Min(Output.Count / (End.Y - Begin.Y), CurrentPage + 1);
                        Refresh();
                        break;

                    default:
                        CurrentPage = 0;
                        Write(keyInfo.KeyChar.ToString());
                        break;
                }
                prevKeyInfo = keyInfo;
            } while (keyInfo.Key != ConsoleKey.Enter);

            Reading = false;
            CurrentPage = 0;
            Refresh();
            return CurrentLine;
        }

        public virtual void Refresh() {
            Clear();

            var i2 = 0;
            for (var i = Output.Count - CurrentPage * (End.Y - Begin.Y) - 1; i > 0; i--) 
            {
                var list =
                    (from Match m in Regex.Matches(Output[i], @".{1," + (End.X - Begin.X) + "}")
                     select m.Value).Reverse();

                foreach (var s in list) 
                {
                    ConsoleWriteHelper.Write(s, Begin.X, End.Y - i2 - 1, ConsoleColor.Gray);
                    i2++;

                    if (i2 > End.Y - Begin.Y - 1)
                    {
                        return;
                    }
                }
            }
        }

        public void Clear() {
            for (var y = 0; y < End.Y - Begin.Y; y++) {
                ClearLine(y);
            }
        }

        public void ClearLine(int line) {
            if (line >= End.Y - Begin.Y || line < 0)
                throw new ArgumentOutOfRangeException();

            ConsoleWriteHelper.Write(
                new string(' ', End.X - Begin.X), 
                Begin.X, Begin.Y + line,
                ConsoleColor.Gray);
        }

        private void DeleteSymbols(int n = 1) {
            if (n == 0) return;
            var len = Output.Last().Length;
            if (n < 0 || n > len) throw new ArgumentOutOfRangeException();

            Output[Output.Count - 1] = Output.Last().Substring(0, len - n);
            Refresh();
        }
    }
}