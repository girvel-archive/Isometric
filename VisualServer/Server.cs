using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using IsometricCore.Modules;
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

        public Encoding Encoding { get; } = Encoding.GetEncoding(1251);

        public IPAddress ServerAddress { get; set; }

        public List<Account> Accounts { get; set; }
        
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
            var banned = new Account("banned", "", "-", AccountPermission.User);
            Accounts = new List<Account> 
            {
                new Account("usr", "1", "", AccountPermission.User),
                banned,
            };
            banned.PermanentlyBanned = true;
            #endif
        }



        public bool TryToAutoConnect()
        {
            return new[]
            {
                AddressFamily.InterNetwork,
                AddressFamily.InterNetworkV6,
            }
            .SelectMany(currentFamily => Dns.GetHostEntry(Dns.GetHostName())
            .AddressList
            .Where(a => a.AddressFamily == currentFamily))
            .Any(TryToConnect);
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

            _listenSocket = new Socket(ipPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

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
                throw new InvalidOperationException(
                    "Server is not connected. Use TryToAutoConnect() and TryToConnect()");
            }

            while (true)
            {
                try
                {
                    var socket = _listenSocket.Accept();
                    OnAcceptedConnection?.Invoke();

                    CommandManager.Instance.Interface.GetExecutor(
                        socket.ReceiveAll(Encoding),
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
    }
}

