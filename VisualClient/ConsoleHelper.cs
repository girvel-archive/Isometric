using System;
using System.Collections.Generic;
using System.Linq;
using VectorNet;
using VisualConsole;
using VisualConsole.Exceptions;
using VisualServer;
using static VisualClient.Program;

namespace VisualClient
{
    public static class ConsoleHelper
    {
        public static int PrevWidth = 23;
        public static int PrevHeight = 80;

        public static int Height = 23;
        public static int Width = 80;
        public static ConsoleUI Interface;

        public static IntVector MainOutputAreaBegin => new IntVector(Width / 3 * 2 + 3, 1);

        public static IntVector MainOutputAreaEnd => new IntVector(Width - 1, Height);

        public static IntVector AdditionalOutputAreaBegin => new IntVector(1, 1);

        public static IntVector AdditionalOutputAreaEnd => new IntVector(Width / 3 * 2, Height);



        static ConsoleHelper()
        {

            Interface = new ConsoleUI(new List<ConsoleCommand> {
                new ConsoleCommand(
                    new [] {"new-acc", "na"},
                    "creates new account",
                    "@login,password,email,permission",
                    _newAccount),

                new ConsoleCommand(
                    new [] {"view-accs", "va"},
                    "views all accounts",
                    "",
                    _viewAccounts),

                new ConsoleCommand(
                    new [] {"clear", "cl"},
                    "clears the console",
                    "",
                    _clear),

                new ConsoleCommand(
                    new [] {"size", "sz"},
                    "sets the size of global output area",
                    "@width,height",
                    _setSize),

                new ConsoleCommand(
                    new [] {"size-get", "sg"},
                    "gets the size of global output area",
                    "",
                    _getSize),

                new ConsoleCommand(
                    new [] {"frequency", "fq"},
                    "sets the frequency of refreshing game output area",
                    "@period",
                    _setViewingFrequency),

                new ConsoleCommand(
                    new [] {"blist-add", "ba"},
                    "adds type of log message to blacklist",
                    "@type",
                    _blackListAdd),

                new ConsoleCommand(
                    new [] {"blist-remove", "br"},
                    "removes type of log message from blacklist",
                    "@type",
                    _blackListRemove),

                new ConsoleCommand(
                    new [] {"save", "sv"},
                    "saves current session",
                    "",
                    _save),

                new ConsoleCommand(
                    new[] {"open", "on"},
                    "opens new session from file",
                    "",
                    _open),
                #if !DEBUG
                new ConsoleCommand(
                new[] {"blist-add-type", "bat"},
                "adds type of exception to blacklist",
                "@type",
                _blackListAddType),

                new ConsoleCommand(
                new[] {"blist-remove-type", "brt"},
                "removes exception type from blacklist",
                "@type",
                _blackListRemoveType),
                #endif
            },
                new OutputDiv(
                    MainOutputAreaBegin,
                    MainOutputAreaEnd,
                    null, ">>"));
        }

        public static void Init()
        {
            Interface.AdditionalOutputAreas.Add(
                new OutputDivGrid(
                    AdditionalOutputAreaBegin,
                    AdditionalOutputAreaEnd,
                    Interface, Territory));
        }

        private static void _newAccount(CommandMetaData meta)
        {
            AccountPermission perm;
            try
            {
                perm = (AccountPermission) Enum.Parse(typeof(AccountPermission), meta.Args[3]);
            }
            catch (ArgumentException)
            {
                throw new WrongCommandException("newAccount");
            }
            MainServer.Accounts.Add(new Account(
                meta.Args[0], meta.Args[1], meta.Args[2], perm, Game.World, Game));
        }

        private static void _viewAccounts(CommandMetaData meta)
        {
            MainLog?.Write(( 
                from a in MainServer.Accounts
                select a.ToString() ).Aggregate((cur, acc) => cur + "\n" + acc),
                LogType.User);
        }

        private static void _clear(CommandMetaData meta)
        {
            Console.Clear();
            meta.Interface.OutputArea.Refresh();
            foreach (var area in meta.Interface.AdditionalOutputAreas)
            {
                area.Refresh();
            }
        }

        private static void _setSize(CommandMetaData meta)
        {
            Console.Clear();
            try
            {
                Width = Convert.ToInt32(meta.Args[0]);
                Height = Convert.ToInt32(meta.Args[1]);

                meta.Interface.OutputArea.Begin = MainOutputAreaBegin;
                meta.Interface.OutputArea.End = MainOutputAreaEnd;

                meta.Interface.AdditionalOutputAreas.First().Begin = AdditionalOutputAreaBegin;
                meta.Interface.AdditionalOutputAreas.First().End = AdditionalOutputAreaEnd;

                meta.Interface.OutputArea.Refresh();
                meta.Interface.AdditionalOutputAreas.First().Refresh();

                PrevWidth = Width;
                PrevHeight = Height;
            }
            catch (ArgumentOutOfRangeException)
            {
                Width = PrevWidth;
                Height = PrevHeight;

                meta.Interface.OutputArea.Begin = MainOutputAreaBegin;
                meta.Interface.OutputArea.End = MainOutputAreaEnd;

                meta.Interface.AdditionalOutputAreas.First().Begin = AdditionalOutputAreaBegin;
                meta.Interface.AdditionalOutputAreas.First().End = AdditionalOutputAreaEnd;

                meta.Interface.OutputArea.Refresh();
                meta.Interface.AdditionalOutputAreas.First().Refresh();

                Interface.OutputArea.WriteLine("Wrong value!");
            }
        }

        private static void _getSize(CommandMetaData meta)
        {
            meta.Interface.OutputArea.WriteLine($"Size: ({Width}, {Height})");
        }

        private static void _setViewingFrequency(CommandMetaData meta)
        {
            meta.Interface.AdditionalOutputAreasRefreshDelay = Convert.ToInt32(meta.Args[0]);
        }

        private static void _blackListAdd(CommandMetaData meta)
        {
            LogType type;
            try
            {
                Enum.TryParse(meta.Args[2], true, out type);
            }
            catch (ArgumentException)
            {
                throw new WrongCommandException("blist-add");
            }
            MainLog.BlackList.Add(type);
        }

        private static void _blackListRemove(CommandMetaData meta)
        {
            LogType type;
            try
            {
                type = (LogType) Enum.Parse(typeof(LogType), meta.Args[2]);
            }
            catch (ArgumentException)
            {
                throw new WrongCommandException("blist-remove");
            }
            MainLog.BlackList.Remove(type);
        }

        private static void _save(CommandMetaData meta)
        {
            Save();
        }

        private static void _open(CommandMetaData meta)
        {
            Open();
        }

#if !DEBUG
        private static void _blackListAddType(CommandMetaData meta)
        {
            var type = Type.GetType(meta.Args[0], false);
            if (type != null)
            {
                MainLog.TypeBlackList.Add(type);
            }
            else
            {
                throw new WrongCommandException("blist-add-type");
            }
        }

        private static void _blackListRemoveType(CommandMetaData meta)
        {
            var type = Type.GetType(meta.Args[0], false);
            if (type != null && MainLog.TypeBlackList.Contains(type))
            {
                MainLog.TypeBlackList.Remove(type);
            }
            else
            {
                throw new WrongCommandException("blist-remove-type");
            }
        }
#endif
    }
}