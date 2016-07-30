using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using CommandInterface;
using CompressedStructures;
using GameBasics;
using VisualClient;



namespace VisualServer
{
    [Serializable]
    public class Server
    {
        public class NetArgs
        {
            public Socket CurrentSocket { get; }
            public Server MainServer { get; }

            public NetArgs(Socket currentSocket, Server mainServer)
            {
                CurrentSocket = currentSocket;
                MainServer = mainServer;
            }
        }



        [NonSerialized]
        internal Dictionary<Account, Connection> CurrentConnections;

        [NonSerialized]
        private Socket _listenSocket;

        [NonSerialized]
        private Interface<NetArgs> Interface;



        public int ServerPort { get; set; } = 8005;
        public string ServerAddress { get; set; } = "192.168.0.100";

        public List<Account> Accounts { get; set; }
        public ILog MainLog { get; set; }



        public int SpamErrorsMax { get; set; }



        public const string SmtpHost = "smtp.yandex.ru";
        public const int SmtpPort = 465;
        public const bool SmtpEnableSsl = true;

        public const string 
            ServerEmail = "widauka@ya.ru",
            ServerPassword = "SOWhatisitFF0912";



        static Server() {}



        public Server()
        {
            CurrentConnections = new Dictionary<Account, Connection>();

            Interface = new Interface<NetArgs>(
                new List<Command<NetArgs>> {
                    new Command<NetArgs>(
                        "ln",
                        "(ln -> LogiN) checks received login and password, " +
                            "authorizes user",
                        "@account",
                        _login),

                    new Command<NetArgs>(
                        "esn",
                        "(esn -> Email Send Numbers) sends random numbers to user " +
                            "email for signup",
                        "@email",
                        _emailSendNumbers),
                });
        }



        public Server(ILog mainLog, World world, Game game) : this()
        {
            MainLog = mainLog;
            Accounts = new List<Account> {
                new Account("main", "UI!@QWaszx", "widauka@ya.ru", AccountPermission.Admin, world, game),
                new Account("usr", "1", "", AccountPermission.User, world, game)
            };
        }

        public void ServerLoop()
        {
            IPEndPoint ipPoint;
            try
            {
                ipPoint = new IPEndPoint(IPAddress.Parse(ServerAddress), ServerPort);
            }
            catch (FormatException)
            {
                MainLog?.Write("Format exception!", LogType.User);
                return;
            }
            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Unspecified);

            _listenSocket.Bind(ipPoint);
            _listenSocket.Listen(10); // TODO increase if there will be a lot of players
            MainLog?.Write("Server launched. Waiting for connections...", LogType.User);

            while (true)
            {
#if !DEBUG
                try
                {
#endif
                    var acceptedConnection = _listenSocket.Accept();
                    MainLog?.Write("Connection accepted", LogType.Connection);

                    var currentStringBuilder = new StringBuilder();
                    var receivedData = new byte[4096];

                    do
                    {
                        var bytes = acceptedConnection.Receive(receivedData);
                        
                        currentStringBuilder.Append(Encoding.ASCII.GetString(
                            receivedData, 0, bytes));
                    }
                    while (acceptedConnection.Available > 0);

                    Interface.UseCommand(
                        currentStringBuilder.ToString(), 
                        new NetArgs(acceptedConnection, this));
#if !DEBUG
                }
                catch (Exception e)
                {
                    MainLog?.Exception(e, false);
                }
#endif
            }
        }

        public void CheckVersion(ProgramVersion version)
        {

        }



        private void _login(string[] args, NetArgs netArgs)
        {
            var receivedAccount = CommonAccount.GetFromBytes(
                Encoding.ASCII.GetBytes(args[0]));
            
            var suitableAccounts =
                from a in netArgs.MainServer.Accounts
                where a.Email == receivedAccount.Email &&
                        a.Password == receivedAccount.Password
                select a;

            if (suitableAccounts.First().PermanentlyBanned)
            { 
                MainLog.Write(
                    "Unsuccessful attempt to enter by BANNED account data" +
                        $"Email: {receivedAccount.Email}" +
                        $"Password: {receivedAccount.Password}",
                    LogType.Spam);
            }

            var succesful = suitableAccounts.Any();

            netArgs.CurrentSocket.Send(Encoding.ASCII.GetBytes(
                "ln-r@" + ( succesful ? "1" : "0" )));

            MainLog?.Write(
                ( succesful ? "S" : "Uns" ) + $"uccesful attempt to enter" +
                    $"\n\tEmail:    {receivedAccount.Email}" +
                    $"\n\tPassword: {receivedAccount.Password}", 
                LogType.Connection);

            if (!succesful) return;

            var newConnection = new Connection(
                netArgs.CurrentSocket, MainLog, suitableAccounts.First(), this);
            
            netArgs.MainServer.CurrentConnections[suitableAccounts.First()]
                = newConnection;

            newConnection.Start();
        }

        private void _emailSendNumbers(string[] args, NetArgs netArgs)
        {
            var numbers = GameRandom.Instance.Next(10000, 99999);
            // TODO checking email using
            new SmtpClient {
                Host = SmtpHost,
                Port = SmtpPort,
                EnableSsl = SmtpEnableSsl,
                Credentials = new NetworkCredential(
                    ServerEmail.Split('@')[0],
                    ServerPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
            }.Send(
                new MailMessage(
                    "widauka@ya.ru",
                    args[0].Replace("#", "@"),
                    "Isometric kingdoms registration",
                    "Numbers: " + numbers));
            // TODO login or closing connection (if Exit button pressed)
                 }
    }
}

