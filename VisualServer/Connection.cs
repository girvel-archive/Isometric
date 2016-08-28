using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BinarySerializationExtensions;
using CommandInterface;
using IsometricCore.Modules;
using IsometricCore.Modules.PlayerModule;
using VisualServer.Modules.CommandModule.Connection;
using VisualServer.Modules.SpamModule;
using SocketExtensions;

namespace VisualServer
{
    public class Connection
    {
        public bool Active { get; set; }

        public Thread Thread { get; set; }

        public Socket Socket { get; set; }
        public Server ParentServer { get; set; }

        public Account Account { get; set; }

        public int SpamCounter { get; private set; }
        public int SpamCounterMax { get; set; }

        public Encoding Encoding => ParentServer.Encoding;



        public delegate void DataEvent(string data, Account account);
        public delegate void ConnectionEvent(Connection connection);

        public static event DataEvent OnDataReceived;
        public static event DataEvent OnWrongCommand;
        public static event ConnectionEvent OnConnectionEnd;
        public static event ConnectionEvent OnConnectionAbort;



        #region Ctors, finalizers

        public Connection(
            Socket socket, Account account, Server server)
        {
            Socket = socket;
            Account = account;
        }

        ~Connection()
        {
            Stop();
        }

        #endregion



        public void StartThread()
        {
            Thread = new Thread(Start);
            Thread.Start();

            Account.Player.OnTick += SendResources;
        }

        public void Start()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var receivedString = Socket.ReceiveAll(ParentServer.Encoding);

                        OnDataReceived?.Invoke(receivedString, Account);

                        Func<CommandResult> cmdUse;
                        if (CommandManager.Instance.Interface.TryGetFunc(
                                receivedString, new NetArgs(Socket, this), out cmdUse))
                        {
                            cmdUse();
                        }
                        else
                        {
                            OnWrongCommand?.Invoke(receivedString, Account);
                        }
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
            catch (SocketException)
            {
                OnConnectionEnd?.Invoke(this);
            }
            catch (ThreadAbortException)
            {
                OnConnectionAbort?.Invoke(this);
            }
        }

        public void Stop()
        {
            Socket.Close();
            Thread.Abort();

            Account.Player.OnTick -= SendResources;
        }

        protected void Send(string message)
        {
            Socket.Send(Encoding.GetBytes(message));
        }
        


        internal void SendResources(Player owner)
        {
            // FIXME Unity r -> refresh
            Send("refresh".CreateCommand(owner.CurrentResources.SerializeToString(Encoding)));
        }
    }
}

