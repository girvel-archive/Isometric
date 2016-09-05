using System;
using System.Linq;
using System.Threading;
using IsometricCore.Modules.WorldModule;
using IsometricCore.Modules.WorldModule.Land;
using IsometricImplementation;
using VisualClient.Modules;
using VisualClient.Modules.LogModule;
using VisualServer.Modules;
using VisualClient.Tools;

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

            OpenOrGenerate();
            SelectIP();
            SmtpInitialize();
            StartThreads();

//            SingleUI.Instance.Control();
            Console.ReadKey();
        }



        private static void SelectIP()
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
                Territory = World.Instance.LazyGetTerritory(0, 0);
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
            NetThread = new Thread(SingleServer.Instance.ServerLoop);
            NetThread.Start();

            //RefreshThread = new Thread(ClocksManager.Instance.TickLoop);
            //RefreshThread.Start();

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
