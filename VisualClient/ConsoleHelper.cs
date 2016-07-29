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
        public static ConsoleUI UI { get; set; }



        static ConsoleHelper()
        {
            
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