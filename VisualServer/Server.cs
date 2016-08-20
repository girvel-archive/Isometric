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
using IsometricCore.Modules;
using VisualServer.Modules;
using VisualServer.Extensions;
using IsometricCore.Extensions;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using VisualServer.Extensions.Interfaces;



namespace VisualServer
{
    [Serializable]
    public class Server
    {
        #region Args classes

        public class NetArgs : ISocketContainer
        {
            public Socket Socket { get; }
            public Server Server { get; }

            public NetArgs(Socket socket, Server server)
            {
                Socket = socket;
                Server = server;
            }
        }

        public class EmailArgs
        {
            public int Code { get; }

            public EmailArgs(int code)
            {
                Code = code;
            }
        }

        public class EmailPairArgs
        {
            public NetArgs NetArgs { get; }
            public EmailArgs EmailArgs { get; }

            public EmailPairArgs(NetArgs netArgs, EmailArgs emailArgs)
            {
                NetArgs = netArgs;
                EmailArgs = emailArgs;
            }
        }

        #endregion



        public static readonly Version Version = new Version(0, 4);
        public static Version SavingVersion;



        [NonSerialized]
        internal Dictionary<Account, Connection> CurrentConnections;

        [NonSerialized]
        private Socket _listenSocket;

        [NonSerialized]
        private Interface<NetArgs, CommandResult> MainInterface;

        [NonSerialized]
        private Interface<EmailPairArgs, CommandResult> AfterEmailInterface;

        [NonSerialized]
        private Interface<NetArgs, CommandResult> AfterCodeInterface;

        [NonSerialized]
        private bool _connected;



        public const int ServerPort = 8005;

        public IPAddress ServerAddress { get; set; }
        public Encoding Encoding { get; set; } = Encoding.ASCII;

        public List<Account> Accounts { get; set; } = new List<Account>();



        public const string SmtpHost = "smtp.yandex.ru";
        public const int SmtpPort = 465;
        public const bool SmtpEnableSsl = true;



        public delegate void LoginEvent(string email, LoginResult result);

        public event LoginEvent OnLoginAttempt;
        public event Action OnWrongIP;
        public event Action OnAcceptedConnection;



        static Server() {}

        public Server()
        {
            CurrentConnections = new Dictionary<Account, Connection>();

            MainInterface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "login",
                    new[] { "account" },
                    _login),

                new Command<NetArgs, CommandResult>( // FIXME Unity email-send-code
                    "email-send-code",
                    new[] { "email" },
                    _emailSendCode));

            AfterEmailInterface = new Interface<EmailPairArgs, CommandResult>(
                new Command<EmailPairArgs, CommandResult>(
                    "code-set",
                    new[] { "code" },
                    _codeSet));

            AfterCodeInterface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "account-set",
                    new[] { "login", "account" },
                    _accountSet));

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
                OnWrongIP?.Invoke();
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
                try
                {
                    var socket = _listenSocket.Accept();
                    OnAcceptedConnection?.Invoke();

                    MainInterface.GetFunc(
                        socket.ReceiveAll(this.Encoding), 
                        new NetArgs(socket, this))();

                    // TODO 1.1 spamfilter using
                }
                catch (Exception e)
                {
                    GlobalData.Instance.OnUnknownException?.Invoke(e);

                    #if DEBUG

                    throw;

                    #endif
                }
            }
        }

        [OnDeserialized]
        public void CheckVersion()
        {

        }





        // Main interface
        // @account
        private CommandResult _login(Dictionary<string, string> args, NetArgs netArgs)
        {
            var receivedAccount = Encoding.GetBytes(args["account"]).ByteDeserialize<CommonAccount>();

            var suitableAccounts = netArgs.Server.Accounts.Where(
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

            netArgs.Send("ln-r".CreateCommand(((byte)result).ToString()));

            OnLoginAttempt?.Invoke(receivedAccount.Email, result);

            if (!successful)
            {
                return CommandResult.Unsuccessful;
            }

            var account = suitableAccounts.First();

            var newConnection = new Connection(
                netArgs.Socket, account, this);

            netArgs.Server.CurrentConnections[account] = 
                newConnection;

            newConnection.StartThread();

            return CommandResult.Successful;
        }

        // Main interface
        // @email
        private CommandResult _emailSendCode(Dictionary<string, string> args, NetArgs netArgs)
        {
            var code = SingleRandom.Instance.Next(10000, 99999);

            SmtpManager.SendSignupMail(args["email"], code);

            Func<CommandResult> result;
            var i = 0;

            while (!AfterEmailInterface.TryGetFunc(
                      netArgs.Socket.ReceiveAll(Encoding), 
                      new EmailPairArgs(netArgs, new EmailArgs(code)),
                      out result)
                || result() != CommandResult.Successful)
            {
                if (++i >= 3)
                {
                    return CommandResult.Unsuccessful;
                }
                // TODO 1.x spam filter
            }

            i = 0;

            while (!AfterCodeInterface.TryGetFunc(
                       netArgs.Socket.ReceiveAll(Encoding), 
                       netArgs, out result)
                   || result() == CommandResult.Unsuccessful)
            {
                if (++i >= 3)
                {
                    return CommandResult.Unsuccessful;
                }
            }

            netArgs.Socket.Close();
            
            return CommandResult.Successful;
        }

        // After email interface
        // @code
        private CommandResult _codeSet(Dictionary<string, string> args, EmailPairArgs pairArgs)
        {
            int code;
            if (!int.TryParse(args["code"], out code))
            {
                // FIXME Unity code-result
                pairArgs.NetArgs.Send("code-result".CreateCommand(((byte)CodeResult.WrongCommand).ToString()));
                    
                return CommandResult.Spam;
            }

            if (code == pairArgs.EmailArgs.Code)
            {
                pairArgs.NetArgs.Send("code-result".CreateCommand(((byte)CodeResult.Successful).ToString()));

                return CommandResult.Successful;
            }

            pairArgs.NetArgs.Send("code-result".CreateCommand(((byte)CodeResult.WrongCode).ToString()));

            return CommandResult.Unsuccessful;
        }

        // After code interface
        // @login,account
        private CommandResult _accountSet(Dictionary<string, string> args, NetArgs netArgs)
        {
            var account = Encoding.GetBytes(args["account"]).ByteDeserialize<CommonAccount>();

            if (!Regex.IsMatch(args["login"], @"^[\w\s]*$"))
            {
                netArgs.Send("account-result".CreateCommand(((byte)AccountCreatingResult.WrongLogin).ToString()));

                return CommandResult.Spam;
            }

            if (Accounts.Any(a => a.Email == account.Email))
            {
                netArgs.Send("account-result".CreateCommand(((byte)AccountCreatingResult.ExistingEmail).ToString()));

                return CommandResult.Unsuccessful;
            }

            if (Accounts.Any(a => a.Login == args["login"]))
            {
                netArgs.Send("account-result".CreateCommand(((byte)AccountCreatingResult.ExistingLogin).ToString()));

                return CommandResult.Unsuccessful;
            }

            Accounts.Add(new Account(args["login"], account));
            netArgs.Send("account-result".CreateCommand(((byte)AccountCreatingResult.Successful).ToString()));

            return CommandResult.Successful;
        }
    }
}

