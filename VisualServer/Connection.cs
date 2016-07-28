using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommandInterface;
using CompressedStructures;
using GameBasics;

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
        }



        public bool Active { get; set; }
        
        public Interface<NetArgs> Interface { get; set; }
        public Thread MainThread { get; set; }

        public Socket MainSocket { get; set; }
        public ILog MainLog { get; set; }
        public Server ParentServer { get; set; }

        public Account Account { get; set; }

        public int SpamCounter { get; private set; }
        public int SpamCounterMax { get; set; }



        public Connection(
            Socket mainSocket, ILog mainLog, Account mainAccount, Server server)
        {
            MainSocket = mainSocket;
            MainLog = mainLog;
            Account = mainAccount;

            Interface = new Interface<NetArgs>(
                new List<Command<NetArgs>> {
                    new Command<NetArgs>(
                        "gt",
                        "(gt -> Get Territory) sends information " +
                            "about current territory",
                        "",
                        _getTerritory),

                    new Command<NetArgs>(
                        "gba",
                        "(gba -> Get Building context Actions) sends information " +
                            "about possible building context actions",
                        "@building",
                        _getBuildingContextActions),

                    new Command<NetArgs>(
                        "uba",
                        "(uba -> Use Building context Action) uses building context action",
                        "@action",
                        _useBuildingContextActions),

                    new Command<NetArgs>(
                        "gr",
                        "(gr -> Get Resources) sends current resources",
                        "",
                        (args, netArgs) => _sendResources(null, new Player.RefreshEventArgs(mainAccount.Player))
                    ),
                }
            );
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
                    SpamCounter    = Math.Max(0, SpamCounter - 1);

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
                MainLog?.Write($"End of {Account.Login}'s connection", LogType.Connection);
            }
        }

        private void _sendResources(object sender, Player.RefreshEventArgs args)
        {
            Send("r@" + new CommonResources(
                new Dictionary<ResourceType, int>(args.Owner.Resources.Resource),
                new Dictionary<ResourceType, int>(args.Owner.Resources.LastIncrease))
                    .GetString);
        }

        private void _getTerritory(string[] args, NetArgs netArgs)
        {
            netArgs.CurrentSocket.Send(Encoding.ASCII.GetBytes(
                $"st@{new object[] { Territory.Size, Territory.Size }.ToArgumentType()}," +
                    Account.Player.Territory
                        .Select(b => b.Pattern.Symbol.ToString()).Aggregate((s, b) => s + b)));
        }

        // @building
        private void _getBuildingContextActions(string[] args, NetArgs netArgs)
        {
            var p = CommonBuilding.GetFromString(args[0]).Position;
            MainLog?.Write($"Position: ({p.X};{p.Y})", LogType.User);
            var pattern = Account.Player.Territory[CommonBuilding.GetFromString(args[0]).Position].Pattern;
            var player = Account.Player;

            try
            {
                netArgs.CurrentSocket.Send(Encoding.ASCII.GetBytes("sba@" +
                    player.Game.BuildingGraph.Find(pattern)[0].Children.Select(
                        c => new CommonBuildingAction(
                            player.Resources.Enough(c.Value.NeedResources),
                            $"Upgrade to {c.Value.Name}")).ToArgumentList(e => e.GetString)));
            }
            catch (IndexOutOfRangeException) {}
        }

        // @action
        private void _useBuildingContextActions(string[] args, NetArgs netArgs)
        {
            
        }
    }
}

