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
using VisualServer.Modules.CommandModule.Server;
using SocketExtensions;



namespace VisualServer
{
    [Serializable]
    public class Server
    {
        public static readonly Version Version = new Version(0, 4);
        public static Version SavingVersion;



        [NonSerialized]
        public List<Connection> CurrentConnections;

        public Encoding Encoding { get; private set; } = Encoding.ASCII;

        public IPAddress ServerAddress { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();
        
        [NonSerialized]
        private bool _connected;
        public bool Connected
        {
            get
            {
                return _connected;
            }
            private set
            {
                _connected = value;
            }
        }



        public const string SmtpHost = "smtp.yandex.ru";
        public const int SmtpPort = 465;
        public const bool SmtpEnableSsl = true;

        public const int ServerPort = 8005;



        [NonSerialized] 
        private Socket _listenSocket;



        public event Action OnWrongIP;
        public event Action OnAcceptedConnection;



        static Server() {}

        public Server()
        {
            CurrentConnections = new List<Connection>();

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

            Connected = true;
            ServerAddress = ip;

            return true;
        }

        public void ServerLoop()
        {
            if (!Connected)
            {
                throw new InvalidOperationException("Server is not connected. Use TryToAutoConnect() and TryToConnect()");
            }

            while (true)
            {
                try
                {
                    var socket = _listenSocket.Accept();
                    OnAcceptedConnection?.Invoke();

                    CommandManager.Instance.Interface.GetFunc(
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

    }
}

