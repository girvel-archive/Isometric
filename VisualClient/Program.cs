using System;
using System.Threading;
using GameCore.Modules.WorldModule.Land;
using System.IO;
using VisualClient.Modules;
using System.Runtime.Serialization.Formatters.Binary;
using GameCore.Modules.WorldModule;
using System.Runtime.Serialization;
using GameCore.Modules.TickModule;
using VisualServer;
using GameRealization;
using VisualClient.Modules.LogModule;

namespace VisualClient
{
    internal static class Program
    {
        public static Thread
            NetThread,
            RefreshThread,
            SavingThread;

        public static Territory Territory { get; set; }

        public static int SavingPeriodMilliseconds { get; set; } = 60000;

        public static string SavingDirectory { get; set; } = @"saves";
        public static string SavingFile { get; set; } = @"server-save";
        public static string SavingPathLog { get; set; } = @"server-log";



        static Program()
        {
            InitializationManager.Init();
        }



        private static void Main(string[] args)
        {
            Console.Clear();


            // Generation | Opening:

            if (TryOpen())
            {
                Log.Instance.Write("Saves file were opened");
            }
            else
            {
                Territory = World.Instance.LazyGetTerritory(0, 0);
                Log.Instance.Write("Main territory is generated");
            }


            // Ip:

            if (SingleServer.Instance.TryToAutoConnect())
            {
                Log.Instance.Write("Server IP selected automatically");
            }
            else
            {
                Log.Instance.Write("Server IP automatic selection failed");

                while (true)
                {
                    Console.Write("Write your IP: ");
                    var ip = Console.ReadLine();

                    if (SingleServer.Instance.TryToConnect(ip))
                    {
                        break;
                    }

                    Log.Instance.Write("Wrong IP format");
                }
            }

            Log.Instance.Write($"IP set as {SingleServer.Instance.ServerAddress}");


            // Threads:

            NetThread = new Thread(SingleServer.Instance.ServerLoop);
            NetThread.Start();

            RefreshThread = new Thread(ClocksManager.Instance.TickLoop);
            RefreshThread.Start();

            SavingThread = new Thread(_savingLoop);
            SavingThread.Start();

//            SingleUI.Instance.Control();
            Console.ReadKey();
        }



        public static bool TrySave()
        {
            try
            {
                if (!Directory.Exists(SavingDirectory))
                {
                    Directory.CreateDirectory(SavingDirectory);
                }

                using (FileStream mainStream = File.OpenWrite(
                    $"{SavingDirectory}/{SavingFile}"))
                {
                    var serializer = new BinaryFormatter();

                    foreach (var @object in new object[] {
                        SingleServer.Instance,
                        World.Instance,
                        SavingPeriodMilliseconds})
                    { 
                        serializer.Serialize(mainStream, @object);
                    }

                    Log.Instance.Write("Saved successful");

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Exception(ex, "Saving exception was catched");

                #if DEBUG
                throw;
                #endif

                return false;
            }
        } 

        public static bool TryOpen()
        {
            try
            {
                if (!File.Exists($"{SavingDirectory}/{SavingFile}"))
                {
                    return false;
                }

                using (FileStream mainStream = File.OpenRead(
                    $"{SavingDirectory}/{SavingFile}"))
                {
                    var serializer = new BinaryFormatter();

                    SingleServer.Instance = (Server) serializer.Deserialize(mainStream);
                    World.Instance = (World) serializer.Deserialize(mainStream);
                    SavingPeriodMilliseconds = (int) serializer.Deserialize(mainStream);
                }
                CheckVersion();

                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.Exception(ex, "Opening exception was catched");

                #if DEBUG
                throw;
                #else
                return false;
                #endif
            }
        }



        private static void CheckVersion()
        {
            Log.Instance.CheckVersion();
            SingleServer.Instance.CheckVersion();
            World.Instance.CheckVersion();
        }

        private static void _savingLoop()
        {
            while (true)
            {
                if (TrySave())
                {
                    Log.Instance.Write("Game was saved successful");
                }
                else
                {
                    Log.Instance.Write("Problem with game saving");
                }

                Thread.Sleep(SavingPeriodMilliseconds);
            }
        }
    }
}
