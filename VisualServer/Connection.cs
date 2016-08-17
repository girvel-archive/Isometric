﻿using System;
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

namespace VisualServer
{
    public class Connection
    {
        #region Login classes

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

        #endregion



        public bool Active { get; set; }
        
        public Interface<NetArgs, CommandResult> Interface { get; set; }
        public Thread MainThread { get; set; }

        public Socket MainSocket { get; set; }
        public Server ParentServer { get; set; }

        public Account Account { get; set; }

        public int SpamCounter { get; private set; }
        public int SpamCounterMax { get; set; }



        public delegate void DataEvent(string data, Account account);
        public delegate void ConnectionEvent(Connection connection);

        public static event DataEvent OnDataReceived;
        public static event DataEvent OnWrongCommand;
        public static event ConnectionEvent OnConnectionEnd;



        #region Ctors, finalizers

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
                    _useBuildingContextAction),
            
                new Command<NetArgs, CommandResult>(
                    "gr", new string[0],
                    _sendResources));
        }

        ~Connection()
        {
            Stop();
        }

        #endregion



        public void StartThread()
        {
            MainThread = new Thread(Start);
            MainThread.Start();

            Account.Player.OnTick += _sendResources;
        }

        public void Start()
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

                    var receivedString = MainSocket.ReceiveAll(ParentServer.Encoding);

                    OnDataReceived?.Invoke(receivedString, Account);

                    Func<CommandResult> cmdUse;
                    if (Interface.TryGetFunc(receivedString, new NetArgs(MainSocket), out cmdUse))
                    {
                        cmdUse();
                    }
                    else
                    {
                        OnWrongCommand?.Invoke(receivedString, Account);
                    }

                    #if !DEBUG
                    }
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
        }

        public void Stop()
        {
            MainSocket.Close();
            MainThread.Abort();

            Account.Player.OnTick -= _sendResources;
        }

        protected void Send(string message)
        {
            MainSocket.Send(Encoding.ASCII.GetBytes(message));
        }
        


        private void _sendResources(Player owner)
        {
            // FIXME DebugCreateCommand -> CreateCommand
            Send(this.Interface.DebugCreateCommand("r",
                owner.CurrentResources.SerializeToBytes().ToASCII()));
        }

        private CommandResult _sendResources(Dictionary<string, string> args, NetArgs netArgs)
        {
            _sendResources(Account.Player);

            return CommandResult.Successful;
        }

        private CommandResult _getTerritory(Dictionary<string, string> args, NetArgs netArgs)
        {
            // FIXME unity st@size,buildings -> st@common_territory
            // FIXME DebugCreateCommand -> CreateCommand
            Send(this.Interface.DebugCreateCommand("st",
                Account.Player.Territory.ToCommon().SerializeToBytes().ToASCII()));

            return CommandResult.Successful;
        }

        // @building
        private CommandResult _getBuildingContextActions(Dictionary<string, string> args, NetArgs netArgs)
        {
            var position = Encoding.ASCII.GetBytes(args["building"]).ByteDeserialize<IntVector>();
            var building = Account.Player.Territory[position];
            var pattern = building.Pattern;
            var patternNodes = BuildingGraph.Instance.Find(pattern);

            if (patternNodes.Any())
            {
                // FIXME DebugCreateCommand -> CreateCommand
                netArgs.SendASCII(this.Interface.DebugCreateCommand("sba",
                    patternNodes[0].Children.Select(
                        c => new CommonBuildingAction(
                            Account.Player.CurrentResources.Enough(c.Value.NeedResources)
                            && c.Value.UpgradePossible(pattern, building),
                            $"Upgrade to {c.Value.Name}"))
                    .SerializeToBytes().ToASCII()));
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

