using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommandInterface;
using VisualServer.Modules.SpamModule;
using GameCore.Modules.PlayerModule;
using CommonStructures;
using BinarySerializationExtensions;
using VisualServer.Extensions;
using VectorNet;
using GameCore.Modules;
using GameCore.Extensions;
using VisualServer.Extensions.Interfaces;

namespace VisualServer
{
    public class Connection
    {
        #region Login classes

        public class NetArgs : ISocketContainer
        {
            public Socket Socket { get; }
            public Connection Connection { get; }

            public Server Server => Connection.ParentServer;

            public NetArgs(Socket currentSocket, Connection connection)
            {
                Socket = currentSocket;
                Connection = connection;
            }
        }

        #endregion



        public bool Active { get; set; }
        
        public Interface<NetArgs, CommandResult> Interface { get; set; }
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

            Interface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "get-territory", new string[0],
                    _getTerritory),

                new Command<NetArgs, CommandResult>(
                    "get-building-action", new[] { "building" },
                    _getBuildingContextActions),
            
                new Command<NetArgs, CommandResult>(
                    "use-building-action", new[] { "action" },
                    _useBuildingContextAction),
            
                new Command<NetArgs, CommandResult>(
                    "get-resources", new string[0],
                    _sendResources));
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

            Account.Player.OnTick += _sendResources;
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
                        if (Interface.TryGetFunc(receivedString, new NetArgs(Socket, this), out cmdUse))
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

            Account.Player.OnTick -= _sendResources;
        }

        protected void Send(string message)
        {
            Socket.Send(Encoding.GetBytes(message));
        }
        


        private void _sendResources(Player owner)
        {
            Send("r".CreateCommand(owner.CurrentResources.SerializeToString(Encoding)));
        }

        private CommandResult _sendResources(Dictionary<string, string> args, NetArgs netArgs)
        {
            _sendResources(Account.Player);

            return CommandResult.Successful;
        }

        private CommandResult _getTerritory(Dictionary<string, string> args, NetArgs netArgs)
        {
            // FIXME unity st@size,buildings -> st@common_territory
            Send("st".CreateCommand( Account.Player.Territory.ToCommon().SerializeToString(Encoding)));

            return CommandResult.Successful;
        }

        // @building
        private CommandResult _getBuildingContextActions(Dictionary<string, string> args, NetArgs netArgs)
        {
            var position = Encoding.GetBytes(args["building"]).ByteDeserialize<IntVector>();
            var building = Account.Player.Territory[position];
            var pattern = building.Pattern;
            var patternNodes = BuildingGraph.Instance.Find(pattern);

            if (patternNodes.Any())
            {
                netArgs.Send("sba".CreateCommand(
                    patternNodes[0].Children.Select(
                        c => new CommonBuildingAction(
                            Account.Player.CurrentResources.Enough(c.Value.NeedResources)
                            && c.Value.UpgradePossible(pattern, building),
                            $"Upgrade to {c.Value.Name}"))
                    .SerializeToString(Encoding)));
            }

            return CommandResult.Successful;
        }

        // @action
        private CommandResult _useBuildingContextAction(Dictionary<string, string> args, NetArgs netArgs)
        {
            throw new NotImplementedException();
        }
    }
}

