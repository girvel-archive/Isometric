using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Girvel.Graph;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Server
{
    [Serializable]
    public class Server
    {
        public static readonly Version Version = new Version(0, 4);
        public static Version SavingVersion;



        [NonSerialized]
        public HashSet<Connection> CurrentConnections;

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

        internal Graph<BuildingPattern> Graph { get; set; }



        [NonSerialized] private Socket _listenSocket;
        

        
        public event Action OnAcceptedConnection;

        public event Action<string> OnWrongCommand;



        static Server() {}

        public Server(World world, PlayersManager playersManager, Graph<BuildingPattern> graph)
        {
            World = world;
            PlayersManager = playersManager;
            Graph = graph;

            CurrentConnections = new HashSet<Connection>();

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

        public bool TryToConnect(IPAddress ip)
        {
            _listenSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _listenSocket.Bind(new IPEndPoint(ip, ServerPort));
            _listenSocket.Listen(10);

            Connected = true;
            ServerAddress = ip;

            return true;
        }
        
        public void Start()
        {
#if DEBUG
            if (!Connected)
            {
                throw new InvalidOperationException(
                    "Server is not connected. Use TryToAutoConnect() and TryToConnect()");
            }
#endif

            while (true)
            {
#if !DEBUG
                try
#endif
                {
                    var socket = _listenSocket.Accept();
                    OnAcceptedConnection?.Invoke();

                    CurrentConnections.Add(new Connection(socket, this));
                }

#if !DEBUG
                catch (Exception ex)
                {
                    Reporter.Instance.ReportError($"Error during loop in {nameof(Server)}.{nameof(Start)}", ex);
                }
#endif
            }
        }
    }
}

