using System;
using System.Collections.Generic;
using System.Linq;
using VectorNet;
using VisualConsole;
using VisualServer;

namespace VisualClient
{
    internal static class ConsoleHelper
    {
        internal static void _newAccount(string[] args, ConsoleUI ui)
        {
            AccountPermission perm;
            try
            {
                perm = (AccountPermission) Enum.Parse(typeof(AccountPermission), args[3]);
            }
            catch (ArgumentException)
            {
                Program.MainLog.Write("wrong value!", LogType.User);
                return;
            }
            Program.MainServer.Accounts.Add(new Account(
                args[0], args[1], args[2], perm, Program.Game.World, Program.Game));
        }

        internal static void _viewAccounts(string[] args, ConsoleUI ui)
        {
            Program.MainLog.Write(( 
                from a in Program.MainServer.Accounts
                select a.ToString() ).Aggregate((cur, acc) => cur + "\n" + acc),
                LogType.User);
        }

        internal static void _clear(string[] args, ConsoleUI ui)
        {
            Console.Clear();
        }

        internal static void _blackListAdd(string[] args, ConsoleUI ui)
        {
            LogType type;
            try
            {
                Enum.TryParse(args[2], true, out type);
            }
            catch (ArgumentException)
            {
                Program.MainLog.Write("wrong value!", LogType.User);
                return;
            }
            Program.MainLog.BlackList.Add(type);
        }

        internal static void _blackListRemove(string[] args, ConsoleUI ui)
        {
            LogType type;
            try
            {
                type = (LogType) Enum.Parse(typeof(LogType), args[2]);
            }
            catch (ArgumentException)
            {
                Program.MainLog.Write("wrong value!", LogType.User);
                return;
            }
            Program.MainLog.BlackList.Remove(type);
        }

        internal static void _save(string[] args, ConsoleUI ui)
        {
            Program.Save();
        }

        internal static void _open(string[] args, ConsoleUI ui)
        {
            Program.Open();
        }

#if !DEBUG
        internal static void _blackListAddType(string[] args, ConsoleUI ui)
        {
            var type = Type.GetType(args[0], false);
            if (type != null)
            {
                Program.MainLog.TypeBlackList.Add(type);
            }
            else
            {
                Program.MainLog.Write("wrong value!", LogType.User);
            }
        }

        internal static void _blackListRemoveType(string[] args, ConsoleUI ui)
        {
            var type = Type.GetType(args[0], false);
            if (type != null && Program.MainLog.TypeBlackList.Contains(type))
            {
                Program.MainLog.TypeBlackList.Remove(type);
            }
            else
            {
                Program.MainLog.Write("wrong value!", LogType.User);
            }
        }
#endif
    }
}