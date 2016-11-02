﻿using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommandInterface.Extensions;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Server.Modules.CommandModule;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketExtensions;

namespace Isometric.Server
{
    public class Connection
    {
       public Server Server { get; set; }

        public Account Account
        {
            get { return _account; }
            set
            {
                if (_account != null)
                {
                    _account.Player.OnTick -= SendResources;
                }

                if (value != null)
                {
                    value.Player.OnTick += SendResources;
                }

                _account = value;
            }
        }
        private Account _account;

        public Encoding Encoding => Server.Encoding;

        internal readonly Socket Socket;



        public delegate void DataEvent(string data, Account account);
        public delegate void ConnectionEvent(Connection connection);

        public static event DataEvent OnDataReceived;
        public static event DataEvent OnWrongCommand;
        public static event ConnectionEvent OnConnectionEnd;
        public static event ConnectionEvent OnConnectionAbort;



        private readonly CommandManager _commandManager;

        private readonly Thread _thread;



        public Connection(Socket socket, Server server)
        {
            Socket = socket;
            Server = server;

            _thread = new Thread(_loop);
            _thread.Start();

            _commandManager = new CommandManager();
        }

        ~Connection()
        {
            Close();
        }
        
        public void Close()
        {
            Socket.Close();
            if (Account != null)
            {
                Account.Player.OnTick -= SendResources;
            }

            _thread.Abort();
        }

        public void Send(string message)
        {
            Socket.Send(Encoding.GetBytes(message));
        }



        private void _loop()
        {
            try
            {
                while (true)
                {
#if !DEBUG
                    try
#endif
                    {
                        var receivedString = Socket.ReceiveAll(Server.Encoding);

                        OnDataReceived?.Invoke(receivedString, Account);
                        
                        if (!_commandManager.Execute(JObject.Parse(receivedString), this))
                        {
                            OnWrongCommand?.Invoke(receivedString, Account);
                        }
                    }
#if !DEBUG
                    catch (Exception ex) when (!(ex is SocketException || ex is ThreadAbortException))
                    {
                        Reporter.Instance.ReportError($"Error during {nameof(Connection)}.{nameof(_loop)}", ex);
                    }
#endif

                }
            }
            catch (SocketException)
            {
                OnConnectionEnd?.Invoke(this);
                Server.CurrentConnections.Remove(this);
                Close();
            }
            catch (ThreadAbortException)
            {
                OnConnectionAbort?.Invoke(this);
            }
        }
        


        /// <summary>
        /// Method for Player.OnTick event
        /// </summary>
        internal void SendResources(Player player)
        {
            Send("resources".CreateCommand(
                JsonConvert.SerializeObject(
                    player.CurrentResources,
                    Formatting.None)));
        }
    }
}

