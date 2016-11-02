#if !DEBUG
using System;
using Isometric.Core.Modules;
#endif
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommandInterface.Extensions;
using Isometric.Core.Modules.PlayerModule;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketExtensions;

namespace Isometric.Server
{
    public class Connection
    {
       public Server Server { get; set; }

        public Account Account { get; set; }

        public Encoding Encoding => Server.Encoding;

        internal readonly Socket Socket;



        public delegate void DataEvent(string data, Account account);
        public delegate void ConnectionEvent(Connection connection);

        public static event DataEvent OnDataReceived;
        public static event DataEvent OnWrongCommand;
        public static event ConnectionEvent OnConnectionEnd;
        public static event ConnectionEvent OnConnectionAbort;

        

        private readonly Thread _thread;



        public Connection(Socket socket, Server server)
        {
            Socket = socket;
            Server = server;

            _thread = new Thread(_loop);
            _thread.Start();
        }

        ~Connection()
        {
            Close();
        }
        
        public void Close()
        {
            Socket.Close();

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
                        
                        if (!Server.RequestManager.Execute(JObject.Parse(receivedString), this))
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
        public void SendResources(Player player)
        {
            Send("resources".CreateCommand(
                JsonConvert.SerializeObject(
                    player.CurrentResources,
                    Formatting.None)));
        }
    }
}

