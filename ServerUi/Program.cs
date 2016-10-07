using System;
using System.IO;
using System.Linq;
using System.Threading;
using Isometric.Client.Modules;
using Isometric.Client.Modules.LogModule;
using Isometric.Client.Tools;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Land;
using Isometric.Game;
using Isometric.GameDataTools.Exceptions;
using Isometric.Server.Modules;

namespace Isometric.Client
{
    internal static class Program
    {
        public static Thread
            NetThread,
            ClocksThread,
            SavingThread;

        public static Area Area { get; set; }

        public static int SavingPeriodMilliseconds { get; set; } = 60000;

        public const string 
            GameDataFile = @"../game-data.isob",
            SavingDirectory = @"saves",
            SavingFile = @"server-save",
            SavingPathLog = @"server-log";



        static Program()
        {
            try
            {
                using (var stream = File.OpenRead(GameDataFile))
                {
                    InitializationManager.Init(stream);
                }
            }
            catch (InvalidGameDataException ex)
            {
                Log.Instance.Exception(ex, $"Invalid game data: '{ex.Message}'");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }



        private static void Main(string[] args)
        {
            Console.Clear();

            OpenOrGenerate();
            SelectIp();
            SmtpInitialize();
            StartThreads();

            LogEvents.Init();

            //            SingleUI.Instance.Control();
            Console.ReadKey();
        }



        private static void SelectIp()
        {
            if (SingleServer.Instance.TryToAutoConnect())
            {
                Log.Instance.Write("Server IP selected automatically");
            }
            else
            {
                Log.Instance.Write("Server IP automatic selection failed");

                while (true)
                {
                    var ip = ConsoleDecorator.GetLine("Write your IP: ");

                    if (SingleServer.Instance.TryToConnect(ip))
                    {
                        break;
                    }

                    Log.Instance.Write("Wrong IP format");
                }
            }

            Log.Instance.Write(
                "IP set as " +
                SingleServer.Instance.ServerAddress
                    .GetAddressBytes()
                    .Aggregate("", (sum, b) => sum + "." + b)
                    .Substring(1));
        }

        private static void OpenOrGenerate()
        {
            #if !DEBUG

            if (SerializationManager.Instance.TryOpen())
            {
                Log.Instance.Write("Saves file were opened");
            }
            else

            #endif
            {
                Area = World.Instance.LazyGetArea(0, 0);
                Log.Instance.Write("Main territory is generated");
            }
        }

        private static void SmtpInitialize()
        {
            try
            {
                SmtpManager.Instance.Connect(
                    "smtp.gmail.com", 25, true,
                    ConsoleDecorator.GetLine("Mail login:    #"),
                    ConsoleDecorator.GetPassword("Mail password: #"));

                Log.Instance.Write("Initialized SmtpManager successfully");
            }
            catch (Exception ex)
            {
                Log.Instance.Exception(ex);

                #if DEBUG

                //throw;

                #endif
            }
        }

        private static void StartThreads()
        {
            NetThread = new Thread(SingleServer.Instance.Start);
            NetThread.Start();

            ClocksThread = new Thread(ClocksManager.Instance.TickLoop);
            ClocksThread.Start();

            //SavingThread = new Thread(_savingLoop);
            //SavingThread.Start();
        }


        private static void _savingLoop()
        {
            while (true)
            {
                Log.Instance.Write(SerializationManager.Instance.TrySave()
                    ? "Game was saved successfully"
                    : "Problem with game saving");

                Thread.Sleep(SavingPeriodMilliseconds);
            }
        }
    }
}
