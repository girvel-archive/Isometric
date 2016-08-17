using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using GameCore.Modules.PlayerModule;
using GameCore.Modules.TickModule;
using GameCore.Modules.WorldModule;
using GameCore.Modules.WorldModule.Land;
using GameRealization;
using VisualClient.Modules;
using VisualClient.Modules.LogModule;
using VisualServer;

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

            if (SerializationManager.Instance.TryOpen())
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







        private static void _savingLoop()
        {
            while (true)
            {
                if (SerializationManager.Instance.TrySave())
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
