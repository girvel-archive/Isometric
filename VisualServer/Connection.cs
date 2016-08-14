using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommandInterface;
using GameBasics;
using VisualServer.Modules.SpamModule;
using GameCore.Modules.PlayerModule;
using CommonStructures;
using BinarySerializationExtensions;
using VisualServer.Extensions;
using VectorNet;
using GameCore.Modules;

namespace VisualServer
{
    internal class Connection : IDisposable
    {
        public class NetArgs
        {
            public Socket CurrentSocket { get; }

            public NetArgs(Socket currentSocket)
            {
                CurrentSocket = currentSocket;
            }

            public void SendASCII(string message)
            {
                CurrentSocket.Send(Encoding.ASCII.GetBytes(message));
            }
        }



        public bool Active { get; set; }
        
        public Interface<NetArgs, CommandResult> Interface { get; set; }
        public Thread MainThread { get; set; }

        public Socket MainSocket { get; set; }
        public Server ParentServer { get; set; }

        public Account Account { get; set; }

        public int SpamCounter { get; private set; }
        public int SpamCounterMax { get; set; }



        public Connection(
            Socket mainSocket, Account mainAccount, Server server)
        {
            MainSocket = mainSocket;
            Account = mainAccount;

            Interface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "gt", new string[0],
                    _getTerritory),

                new Command<NetArgs, CommandResult>(
                    "gba", new[] { "building" },
                    _getBuildingContextActions),
            
                new Command<NetArgs, CommandResult>(
                    "uba", new[] { "action" },
                    _useBuildingContextActions),
            
                new Command<NetArgs, CommandResult>(
                    "gr", new string[0],
                    (args, netArgs) => _sendResources(null, new Player.RefreshEventArgs(mainAccount.Player))));
        }

        ~Connection()
        {
            Stop();
        }

        void IDisposable.Dispose()
        {
            Stop();
        }



        public void Start()
        {
            MainThread = new Thread(_connectionLoop);
            MainThread.Start();

            Account.Player.OnRefresh += _sendResources;
        }

        public void Stop()
        {
            MainSocket.Close();
            MainThread.Abort();

            Account.Player.OnRefresh -= _sendResources;
        }

        protected void Send(string message)
        {
            MainSocket.Send(Encoding.ASCII.GetBytes(message));
        }
        


        private void _connectionLoop()
        {
            Active = true;

            try
            {
                while (Active)
                {
#if !DEBUG
                    try
                    {
#endif
                    SpamCounter = Math.Max(0, SpamCounter - 1);

                    var currentStringBuilder = new StringBuilder();
                    var receivedData = new byte[256];

                    do
                    {
                        var bytes = MainSocket.Receive(receivedData);
                        currentStringBuilder.Append(Encoding.ASCII.GetString(receivedData, 0, bytes));
                    } while (MainSocket.Available > 0);

                    MainLog?.Write("Command received by connection\n\t" + currentStringBuilder, LogType.ReceivedData);
                    try
                    {
                        Interface.UseCommand(currentStringBuilder.ToString(), new NetArgs(MainSocket));
                    }
                    catch (ArgumentException)
                    {
                        MainLog?.Write($"Wrong command {currentStringBuilder.ToString().Split('@')[0]}",
                            LogType.ReceivedData);

                        SpamCounter++;
                        Send("spam-problem");
                    }

                    if (SpamCounter > SpamCounterMax)
                    {
                        Account.SpamErrorTimes++;

                        if (Account.SpamErrorTimes > ParentServer.SpamErrorsMax)
                        {
                            Account.Ban(new TimeSpan(0, 10, 0));
                            Send("spam-ban@0,0,10");
                        }
                        else 
                        {
                            Send("spam-error");
                        }
                        Stop();
                    }
#if !DEBUG
                    }
                    catch (Exception e)
                    {
                        MainLog?.Exception(e, e is SocketException);
                    }
#endif
                }
            }
            catch (SocketException)
            {
                // FIXME OnConnectionEnd
                // MainLog?.Write($"End of {Account.Login}'s connection", LogType.Connection);
            }
        }

        // FIXME using serialization
        private CommandResult _sendResources(object sender, Player.RefreshEventArgs args)
        {
            // FIXME DebugCreateCommand -> CreateCommand
            Send(this.Interface.DebugCreateCommand("r",
                args.Owner.CurrentResources.SerializeToBytes().ToASCII()));
            
            return CommandResult.Successful;
        }

        private CommandResult _getTerritory(Dictionary<string, string> args, NetArgs netArgs)
        {
            // FIXME unity st@size,buildings -> st@common_territory
            // FIXME DebugCreateCommand -> CreateCommand
            Send(this.Interface.DebugCreateCommand("st",
                    Account.Player.Territory.ToCommon()));

            return CommandResult.Successful;
        }

        // @building
        private CommandResult _getBuildingContextActions(Dictionary<string, string> args, NetArgs netArgs)
        {
            var position = Encoding.ASCII.GetBytes(args["building"]).ByteDeserialize<IntVector>();
            var building = Account.Player.Territory[position];
            var pattern = building.Pattern;

            try
            {
                // FIXME DebugCreateCommand -> CreateCommand
                netArgs.SendASCII(this.Interface.DebugCreateCommand("sba",
                    BuildingGraph.Instance.Find(pattern)[0].Children.Select(
                        c => new CommonBuildingAction(
                            Account.Player.CurrentResources.Enough(c.Value.NeedResources)
                            && c.Value.UpgradePossible(pattern, building),
                            $"Upgrade to {c.Value.Name}")).SerializeToBytes()));
            }
            catch (IndexOutOfRangeException) {}

            return CommandResult.Successful;
        }

        // @action
        private CommandResult _useBuildingContextActions(string[] args, NetArgs netArgs)
        {
            throw new NotImplementedException();
        }
    }
}

