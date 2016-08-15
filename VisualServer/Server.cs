using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using CommandInterface;
using CommonStructures;
using BinarySerializationExtensions;
using VisualServer.Modules.SpamModule;
using GameCore.Modules;
using VisualServer.Modules;
using VisualServer.Extensions;



namespace VisualServer
{
    [Serializable]
    public class Server
    {
        #region Args classes

        public class NetArgs
        {
            public Socket CurrentSocket { get; }
            public Server MainServer { get; }

            public NetArgs(Socket currentSocket, Server mainServer)
            {
                CurrentSocket = currentSocket;
                MainServer = mainServer;
            }

            public void SendASCII(string message)
            {
                CurrentSocket.Send(Encoding.ASCII.GetBytes(message));
            }
        }


        public class LoginArgs : EventArgs
        {
            public string Email { get; }
            public LoginResult Result { get; }

            public LoginArgs(string email, LoginResult result)
            {
                Email = email;
                Result = result;
            }
        }

        #endregion



        [NonSerialized]
        internal Dictionary<Account, Connection> CurrentConnections;

        [NonSerialized]
        private Socket _listenSocket;

        [NonSerialized]
        private Interface<NetArgs, CommandResult> Interface;



        public int ServerPort { get; set; } = 8005;
        public string ServerAddress { get; set; } = "176.151.11.21";
        public Encoding Encoding { get; set; } = Encoding.ASCII;

        public List<Account> Accounts { get; set; } = new List<Account>();



        public int SpamErrorsMax { get; set; }



        public const string SmtpHost = "smtp.yandex.ru";
        public const int SmtpPort = 465;
        public const bool SmtpEnableSsl = true;



        public event EventHandler<LoginArgs> OnLoginAttempt;
        public event EventHandler OnFormatException;
        public event EventHandler OnAcceptedConnection;



        static Server() {}



        public Server()
        {
            CurrentConnections = new Dictionary<Account, Connection>();

            Interface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "login",
                    new[] { "account" },
                    _login),

                new Command<NetArgs, CommandResult>(
                    "email-send-numbers",
                    new[] { "email" },
                    _emailSendNumbers));

            #if DEBUG
            Accounts = new List<Account> 
            {
                new Account("usr", "1", "", AccountPermission.User)
            };
            #endif
        }

        public bool Init(string smtpEmail, string smtpPassword)
        {
            try
            {
                SmtpManager.SingleClient = new SmtpClient {
                    Host = SmtpHost,
                    Port = SmtpPort,
                    EnableSsl = SmtpEnableSsl,
                    Credentials = new NetworkCredential(
                        smtpEmail.Split('@')[0],
                        smtpPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                };
            }
            catch (SmtpException)
            {
                return false;
            }
            return true;
        }



        public void ServerLoop()
        {
            IPAddress ip;

            if (!IPAddress.TryParse(ServerAddress, out ip))
            {
                OnFormatException?.Invoke(this, new EventArgs());
                return;
            }

            var ipPoint = new IPEndPoint(IPAddress.Parse(ServerAddress), ServerPort);

            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Unspecified);

            _listenSocket.Bind(ipPoint);
            _listenSocket.Listen(10); // TODO increase if there will be a lot of players

            while (true)
            {
                #if !DEBUG
                try
                {
                #endif

                var socket = _listenSocket.Accept();
                OnAcceptedConnection?.Invoke(this, new EventArgs());

                Interface.GetFunc(
                    socket.ReceiveAll(this.Encoding), 
                    new NetArgs(socket, this))();

                // TODO 1.1 spamfilter using

                #if !DEBUG
                }
                catch (Exception e)
                {
                    MainLog?.Exception(e, false);
                }
                #endif
            }
        }

        public void CheckVersion()
        {

        }



        private CommandResult _login(string[] args, NetArgs netArgs)
        {
            var receivedAccount = Encoding.ASCII.GetBytes(args[0]).ByteDeserialize<CommonAccount>();

            var suitableAccounts = netArgs.MainServer.Accounts.Where(
                a => a.Email == receivedAccount.Email
                && a.Password == receivedAccount.Password);

            var successful = suitableAccounts.Any();
            LoginResult result;

            if (successful)
            {
                if (suitableAccounts.First())
                { 
                    result = LoginResult.Banned;
                }
                else
                {
                    result = LoginResult.Successful;
                }

                return;
            }
            else
            {
                result = LoginResult.Unsuccessful;
            }

            // FIXME debug creating command
            netArgs.SendASCII(this.Interface.DebugCreateCommand("ln-r", (byte)result));

            OnLoginAttempt?.Invoke(this, new LoginArgs(receivedAccount.Email, result));

            if (!successful)
            {
                return CommandResult.Unsuccessful;
            }

            var account = suitableAccounts.First();

            var newConnection = new Connection(
                netArgs.CurrentSocket, account, this);

            netArgs.MainServer.CurrentConnections[account]
            = newConnection;

            newConnection.StartThread();

            return CommandResult.Successful;
        }

        private CommandResult _emailSendNumbers(string[] args, NetArgs netArgs)
        {
//            var numbers = SingleRandom.Instance.Next(10000, 99999);
//
//            SmtpManager.SendSignupMail
            // FIXME _emailSendNumbers()
        }
    }
}

