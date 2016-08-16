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
using GameCore.Extensions;
using System.Runtime.Serialization;



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

        [NonSerialized]
        private bool _connected;



        public const int ServerPort = 8005;

        public IPAddress ServerAddress { get; set; }
        public Encoding Encoding { get; set; } = Encoding.ASCII;

        public List<Account> Accounts { get; set; } = new List<Account>();



        public const string SmtpHost = "smtp.yandex.ru";
        public const int SmtpPort = 465;
        public const bool SmtpEnableSsl = true;



        public event EventHandler<LoginArgs> OnLoginAttempt;
        public event EventHandler OnWrongIP;
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

        // FIXME check all classes for CheckVersion() and create checklist in Program
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

        public bool TryToAutoConnect()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (TryToConnect(ip))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryToConnect(string address)
        {
            IPAddress ip;

            if (!IPAddress.TryParse(address, out ip))
            {
                OnWrongIP?.Invoke(this, new EventArgs());
                return false;
            }

            return TryToConnect(ip);
        }

        public bool TryToConnect(IPAddress ip)
        {
            var ipPoint = new IPEndPoint(ip, ServerPort);

            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Unspecified);

            _listenSocket.Bind(ipPoint);
            _listenSocket.Listen(10); // TODO increase if there will be a lot of players

            _connected = true;
            ServerAddress = ip;

            return true;
        }

        public void ServerLoop()
        {
            if (!_connected)
            {
                throw new InvalidOperationException("Server is not connected. Use TryToAutoConnect() and TryToConnect()");
            }

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
                GlobalData.Instance.OnUnknownException?.Invoke(this, new DelegateExtensions.ExceptionEventArgs(e));
                }
                #endif
            }
        }

        public void CheckVersion()
        {

         }





        // @account
        private CommandResult _login(Dictionary<string, string> args, NetArgs netArgs)
        {
            var receivedAccount = Encoding.ASCII.GetBytes(args["account"]).ByteDeserialize<CommonAccount>();

            var suitableAccounts = netArgs.MainServer.Accounts.Where(
                a => a.Email == receivedAccount.Email
                && a.Password == receivedAccount.Password);

            var successful = suitableAccounts.Any();
            LoginResult result;

            if (successful)
            {
                if (suitableAccounts.First().Banned)
                { 
                    result = LoginResult.Banned;
                }
                else
                {
                    result = LoginResult.Successful;
                }
            }
            else
            {
                result = LoginResult.Unsuccessful;
            }

            // FIXME debug creating command
            netArgs.SendASCII(this.Interface.DebugCreateCommand("ln-r", ((byte)result).ToString()));

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

        // @email
        private CommandResult _emailSendNumbers(Dictionary<string, string> args, NetArgs netArgs)
        {
//            var numbers = SingleRandom.Instance.Next(10000, 99999);
//
//            SmtpManager.SendSignupMail
            // FIXME _emailSendNumbers()

            return CommandResult.Successful;
        }
    }
}

