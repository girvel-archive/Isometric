using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CommandInterface;
using Isometric.Core.Modules;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Server.Modules.CommandModule.Server;
using Isometric.Server.Modules.SpamModule;
using SocketExtensions;

namespace Isometric.Server
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


        internal World World { get; set; }

        internal PlayersManager PlayersManager { get; set; }



        [NonSerialized] private Socket _listenSocket;

        [NonSerialized] private CommandManager _commandManager;
        

        
        public event Action OnAcceptedConnection;
        public event Action OnWrongIp;
        public event Action<string> OnWrongCommand;



        static Server() {}

        public Server(World world, PlayersManager playersManager)
        {
            World = world;
            PlayersManager = playersManager;

            CurrentConnections = new List<Connection>();
            _commandManager = new CommandManager(this);

            #if DEBUG

            string name;
            var banned = new Account(name = "banned", "", "-", AccountPermission.User, new Player(name, World, PlayersManager));
            Accounts = new List<Account> 
            {
                new Account(name = "usr", "1", "", AccountPermission.User, new Player(name, World, PlayersManager)),
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

            if (IPAddress.TryParse(address, out ip))
            {
                return TryToConnect(ip);
            }

            OnWrongIp?.Invoke();
            return false;
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


        public void Start()
        {
            if (!Connected)
            {
                throw new InvalidOperationException(
                    "Server is not connected. Use TryToAutoConnect() and TryToConnect()");
            }

            while (true)
            {
            #if !DEBUG
                try
            #endif
                {
                    var socket = _listenSocket.Accept();
                    OnAcceptedConnection?.Invoke();

                    string data;
                    Executor<CommandResult> executor;

                    if (_commandManager.Interface.TryGetExecutor(
                        data = socket.ReceiveAll(Encoding),
                        new NetArgs(socket, this),
                        out executor))
                    {
                        executor();
                    }
                    else
                    {
                        OnWrongCommand?.Invoke(data);
                    }

                    // TODO 1.1 spamfilter using
                }

            #if !DEBUG
                catch (Exception e)
                {
                    GlobalData.Instance.OnUnknownException?.Invoke(e);
                }
            #endif
            }
        }
    }
}

