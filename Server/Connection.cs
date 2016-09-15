using System.Net.Sockets;
using System.Text;
using System.Threading;
using BinarySerializationExtensions;
using CommandInterface;
using CommandInterface.Extensions;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Server.Modules.CommandModule.Connection;
using Isometric.Server.Modules.SpamModule;
using SocketExtensions;

namespace Isometric.Server
{
    public class Connection
    {
        public bool Active { get; set; }

        public Server ParentServer { get; set; }

        public Account Account { get; set; }

        public Encoding Encoding => ParentServer.Encoding;



        public delegate void DataEvent(string data, Account account);
        public delegate void ConnectionEvent(Connection connection);

        public static event DataEvent OnDataReceived;
        public static event DataEvent OnWrongCommand;
        public static event ConnectionEvent OnConnectionEnd;
        public static event ConnectionEvent OnConnectionAbort;



        private readonly Socket _socket;

        private Thread _thread { get; set; }



        #region Ctors, finalizers

        public Connection(
            Socket socket, Account account, Server server)
        {
            _socket = socket;
            Account = account;
            ParentServer = server;
        }

        ~Connection()
        {
            Stop();
        }

        #endregion



        public void Start()
        {
            _thread = new Thread(_loop);
            _thread.Start();

            Account.Player.OnTick += SendResources;
        }

        public void Stop()
        {
            _socket.Close();
            _thread.Abort();

            Account.Player.OnTick -= SendResources;
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
                        var receivedString = _socket.ReceiveAll(ParentServer.Encoding);

                        OnDataReceived?.Invoke(receivedString, Account);

                        Executor<CommandResult> cmdUse;
                        if (CommandManager.Instance.CommandInterface.TryGetExecutor(
                                receivedString, new NetArgs(_socket, this), out cmdUse))
                        {
                            cmdUse();
                        }
                        else
                        {
                            OnWrongCommand?.Invoke(receivedString, Account);
                        }
                    }
#if !DEBUG

                    catch (Exception e)
                    {
                        GlobalData.Instance.OnUnknownException?.Invoke(e);
                    }

#endif
                }
            }
            catch (SocketException)
            {
                OnConnectionEnd?.Invoke(this);
            }
            catch (ThreadAbortException)
            {
                OnConnectionAbort?.Invoke(this);
            }
        }



        protected void Send(string message)
        {
            _socket.Send(Encoding.GetBytes(message));
        }



        internal void SendResources(Player owner)
        {
            Send("resources".CreateCommand(owner.CurrentResources.Serialize(Encoding)));
        }
    }
}

