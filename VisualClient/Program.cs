using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using IsometricCore.Modules.PlayerModule;
using IsometricCore.Modules.TickModule;
using IsometricCore.Modules.WorldModule;
using IsometricCore.Modules.WorldModule.Land;
using IsometricImplementation;
using VisualClient.Modules;
using VisualClient.Modules.LogModule;
using VisualServer;
using VisualServer.Modules;
using System.Net.Mail;
using System.Net;
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
                    var ip = ConsoleDecorator.GetLine("Write your IP: ");

                    if (SingleServer.Instance.TryToConnect(ip))
                    {
                        break;
                    }

                    Log.Instance.Write("Wrong IP format");
                }
            }

            Log.Instance.Write($"IP set as {SingleServer.Instance.ServerAddress}");


            // Smtp:

            SmtpManager.Instance.Client = new SmtpClient("smtp.gmail.com", 465) 
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    ConsoleDecorator.GetLine("Mail login:    #"),
                    ConsoleDecorator.GetPassword("Mail password: #")),
            };
             


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
