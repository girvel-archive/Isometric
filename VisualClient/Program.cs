using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using GameBasics;
using VisualServer;
using VisualConsole;
using CommandInterface;

namespace VisualClient
{
    internal static class Program
    {
        public static ProgramVersion Version = ProgramVersion.Basic100;

        public static Random MainRandom = new Random();
        public static Log MainLog;

        public static Thread
            NetThread,
            TerrWriteThread,
            RefreshThread,
            SavingThread;

        public static Server MainServer;

        public static Territory Territory { get; set; }
        public static Game Game { get; set; }

        public static int SavingPeriodMilliseconds { get; set; } = 60000;

        public static string SavingDirectory { get; set; } = @"saves";
        public static string SavingPath { get; set; } = @"server-save";
        public static string SavingPathLog { get; set; } = @"server-log";

        public static ConsoleUI UI { get; set; }



        static Program()
        {
            MainLog = new Log();

            UI = new ConsoleUI(new List<Command<ConsoleUI>> {
                new Command(
                    "na",
                    "(new account) creates new account",
                    "@login,password,email,permission",
                    _newAccount),

                new Command(
                    "va",
                    "(view accounts) views all accounts",
                    "",
                    _viewAccounts),

                new Command(
                    "c",
                    "(clear) clears the console",
                    "",
                    _clear),

                new Command(
                    "sz",
                    "sets the size of global output area",
                    "@width,height",
                    _setSize),

                new Command(
                    "size-get",
                    "gets the size of global output area",
                    "",
                    _getSize),

                new Command(
                    "frequency",
                    "sets the frequency of refreshing game output area",
                    "@period",
                    _setViewingFrequency),

                new Command(
                    new [] {"blist-add", "ba"},
                    "adds type of log message to blacklist",
                    "@type",
                    _blackListAdd),

                new Command(
                    new [] {"blist-remove", "br"},
                    "removes type of log message from blacklist",
                    "@type",
                    _blackListRemove),

                new Command(
                    new [] {"save", "sv"},
                    "saves current session",
                    "",
                    _save),

                new Command(
                    new[] {"open", "on"},
                    "opens new session from file",
                    "",
                    _open),
#if !DEBUG
                new Command(
                    new[] {"blist-add-type", "bat"},
                    "adds type of exception to blacklist",
                    "@type",
                    _blackListAddType),

                new Command(
                    new[] {"blist-remove-type", "brt"},
                    "removes exception type from blacklist",
                    "@type",
                    _blackListRemoveType),
#endif
            },
            new Dictionary<ConsoleKey, Action> {
                [ConsoleKey.Escape] = (ui => ui.Mode = UIMode.Messages),
            });
        }



        public static void Save()
        {
#if !DEBUG
            try
            {
#endif
            try
            {
                if (!Directory.Exists(SavingDirectory))
                {
                    Directory.CreateDirectory(SavingDirectory);
                }

                using (
                    FileStream mainStream = File.OpenWrite(
                        $"{SavingDirectory}/{SavingPath}"),
                    logStream = File.OpenWrite($"{SavingDirectory}/{SavingPathLog}"))
                {
                    var serializer = new BinaryFormatter();

                    foreach (var @object in new object[] {
                        Version,
//                        MainServer.Accounts,
                        Game, 
                        Territory,
                        SavingPeriodMilliseconds,
                    }) // TODO Server serialization
                    {
                        serializer.Serialize(mainStream, @object);
                    }

                    serializer.Serialize(logStream, MainLog);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MainLog.Write("Saving: Unathorized access", LogType.IO);
                throw;
            }
            catch (PathTooLongException)
            {
                MainLog.Write("Saving: Path is too long", LogType.IO);
                throw;
            }
            catch (DirectoryNotFoundException)
            {
                MainLog.Write("Saving: Can't find the directory", LogType.IO);
                throw;
            }
            catch (FileNotFoundException)
            {
                MainLog.Write("Saving: Can't find file", LogType.IO);
                throw;
            }
#if !DEBUG
            }
            catch (Exception ex)
            { 
                MainLog.Exception(ex, false);
            }
#endif
        } // TODO remove user argument

        public static void Open()
        {
            try
            {
                using (
                    FileStream mainStream = File.OpenRead(
                        $"{SavingDirectory}/{SavingPath}"),
                    logStream = File.OpenRead($"{SavingDirectory}/{SavingPathLog}"))
                {
                    var serializer = new BinaryFormatter();

                    Version = (ProgramVersion) serializer.Deserialize(mainStream);
                    MainServer.Accounts = (List<Account>) serializer.Deserialize(mainStream);
                    Game = (Game) serializer.Deserialize(mainStream);
                    Territory = (Territory) serializer.Deserialize(mainStream);
                    SavingPeriodMilliseconds = (int) serializer.Deserialize(mainStream);

                    MainLog = (Log) serializer.Deserialize(logStream);
                }
//              CheckVersion();
                MainLog.Write("Saves file opened", LogType.User);
                // TODO enable checkVersion()
            }
            catch (UnauthorizedAccessException)
            {
                MainLog.Write("Saving: Unathorized access", LogType.IO);
                throw;
            }
            catch (PathTooLongException)
            {
                MainLog.Write("Saving: Path is too long", LogType.IO);
                throw;
            }
            catch (DirectoryNotFoundException)
            {
                MainLog.Write("Saving: Can't find directory", LogType.IO);
                throw;
            }
            catch (FileNotFoundException)
            {
                MainLog.Write("Saving: Can't find file", LogType.IO);
                throw;
            }
#if !DEBUG
            catch (SerializationException)
            {
                MainLog.Write("Saving: Serialization error", LogType.IO);
                throw;
            }
#endif
        }



        private static void Main(string[] args)
        {
            try 
            {
                Console.Clear();


                // Openning:

                var successful = false;
                try
                {
                    Open();
                    successful = true;
                }
                catch (FileNotFoundException) {}
                catch (DirectoryNotFoundException) {}
                catch (UnauthorizedAccessException) {}
                catch (SerializationException) {}


                // Generation:
                
                if (!successful)
                {
                    Game = new GameRealization.GameRealization(MainRandom.Next());
                    MainServer = new Server(MainLog, Game.World, Game);

                    Territory = MainServer.Accounts.Find(a => a.Login == "usr")
                        .Player.Territory; // TODO main's territory
                }

                ConsoleHelper.Init();

                if (!successful)
                {
                    MainLog.Write("Main territory is generated", LogType.GameEvent);
                }


                // Ip:

                MainLog.Write("Write your IP: ", LogType.User);
                var ip = ConsoleHelper.Interface.OutputArea.ReadLine();

                if (ip == "")
                {
                    MainLog.Write(
                        "String was empty. Server chose default address",
                        LogType.User);
                }
                else
                {
                    MainServer.ServerAddress = ip;
                }


                // Threads:

                TerrWriteThread = new Thread(
                    ConsoleHelper.Interface.AdditionalOutputLoop);
                TerrWriteThread.Start();

                NetThread = new Thread(MainServer.ServerLoop);
                NetThread.Start();

                RefreshThread = new Thread(Game.RefreshLoop);
                RefreshThread.Start();

                SavingThread = new Thread(_savingLoop);
                SavingThread.Start();

                ConsoleHelper.Interface.Control();
                Console.ReadKey();
            }
            finally
            {
                Console.ReadKey();
            }
        }

        private static void CheckVersion()
        {
            MainLog.CheckVersion(Version);
            MainServer.CheckVersion(Version);
            Game.CheckVersion(Version);
        }

        private static void _savingLoop()
        {
            while (true)
            {
#if !DEBUG
                try
                {
#endif
                    Save();
                    Thread.Sleep(SavingPeriodMilliseconds);
#if !DEBUG
                }
                catch (Exception ex)
                {
                    MainLog.Exception(ex, false);
                }
#endif
            }
        }
    }
}
