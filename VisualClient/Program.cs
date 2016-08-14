﻿using System;
using System.Threading;

namespace VisualClient
{
    internal static class Program
    {
        public static Thread
            NetThread,
            RefreshThread,
            SavingThread;

        public static Server MainServer; // TODO server singleton

        public static Territory Territory { get; set; }

        public static int SavingPeriodMilliseconds { get; set; } = 60000;

        public static string SavingDirectory { get; set; } = @"saves";
        public static string SavingPath { get; set; } = @"server-save";
        public static string SavingPathLog { get; set; } = @"server-log";



        static Program()
        {
            MainLog = new Log();
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
                CheckVersion();

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
                    Game = new GameRealization.Main.GameRealization(MainRandom.Next());
                    MainServer = new Server(MainLog, Game.World, Game);

                    Territory = MainServer.Accounts.Find(a => a.Login == "usr")
                        .Player.Territory; // TODO main's territory
                }

                if (!successful)
                {
                    MainLog.Write("Main territory is generated", LogType.GameEvent);
                }


                // Ip:

                MainLog.Write("Write your IP: ", LogType.User);
                var ip = Console.ReadLine();

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

//                NetThread = new Thread(MainServer.ServerLoop);
//                NetThread.Start();

                RefreshThread = new Thread(Game.RefreshLoop);
                RefreshThread.Start();

                SavingThread = new Thread(_savingLoop);
                SavingThread.Start();

                SingleUI.Instance.Control();
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
