using System;
using System.Collections.Generic;
using System.Linq;
using BinarySerializationExtensions;
using CommandInterface;
using CommandInterface.Extensions;
using Isometric.CommonStructures;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Server.Extensions;
using Isometric.Server.Modules.SpamModule;
using SocketExtensions;

namespace Isometric.Server.Modules.CommandModule.Connection
{
    public class CommandManager
    {
        #region Singleton-part

        [Obsolete("using backing field")]
        private static CommandManager _instance;

        #pragma warning disable 618

        public static CommandManager Instance
        {
            get { return _instance ?? (_instance = new CommandManager()._setDefault()); }

            set
            {
                #if DEBUG

                if (_instance != null)
                {
                    throw new ArgumentException("Instance is already set");
                }

                #endif

                _instance = value;
            }
        }

        #pragma warning restore 618

        #endregion



        public Interface<NetArgs, CommandResult> CommandInterface { get; set; }



        private CommandManager _setDefault()
        {
            CommandInterface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "get-territory", new string[0],
                    _getTerritory),

                new Command<NetArgs, CommandResult>(
                    "get-building-actions", new[] { "building" },
                    _getBuildingContextActions),
            
                new Command<NetArgs, CommandResult>(
                    "upgrade", new[] { "action" },
                    _upgrade),
            
                new Command<NetArgs, CommandResult>(
                    "get-resources", new string[0],
                    _sendResources));

            return this;
        }



        private CommandResult _sendResources(
                Dictionary<string, string> args, NetArgs netArgs)
        {
            SendResources(netArgs);
            return CommandResult.Successful;
        }

        
        private CommandResult _getTerritory(
                Dictionary<string, string> args, NetArgs netArgs)
        {
            netArgs.Send("set-territory"
                .CreateCommand(
                    netArgs.Connection.Account.Player.Territory
                        .ToCommon()
                        .Serialize(netArgs.Connection.Encoding)));

            return CommandResult.Successful;
        }


        // @building
        private CommandResult _getBuildingContextActions(
                Dictionary<string, string> args, NetArgs netArgs)
        {
            var commonBuilding = args["building"]
                .Deserialize<CommonBuilding>(netArgs.Connection.Encoding);

            var building = netArgs.Connection.Account.Player.Territory[commonBuilding.Position];

            var pattern = building.Pattern;
            var patternNodes = BuildingGraph.Instance.Find(pattern);

            if (patternNodes.Any())
            {
                netArgs.Send("set-building-actions".CreateCommand(
                    patternNodes[0].GetChildren().Select(
                        c => new CommonBuildingAction(
                            c.Value.UpgradePossible(
                                netArgs.Connection.Account.Player.CurrentResources,
                                pattern,
                                building),
                            $"Upgrade to {c.Value.Name}",
                            new CommonBuilding(commonBuilding.Position),
                            c.Value.Id))
                        .ToList()
                        .Serialize(netArgs.Connection.Encoding)));
            }

            return CommandResult.Successful;
        }


        // @action
        private CommandResult _upgrade(
                Dictionary<string, string> args, NetArgs netArgs)
        {
            var action = netArgs.Connection.Encoding.GetBytes(args["action"])
                .Deserialize<CommonBuildingAction>();
            var subject = netArgs.Connection.Account.Player.Territory[action.Subject.Position];
            var upgrade = BuildingPattern.Find(action.UpgradeTo);

            var result = subject.TryUpgrade(upgrade, netArgs.Connection.Account.Player)
                ? CommandResult.Successful
                : CommandResult.Unsuccessful;

            if (result == CommandResult.Successful)
            {
                netArgs.Send(
                    "upgrade-result".CreateCommand(
                        new UpgradeResult(
                            subject.Pattern.Id,
                            subject.Position)
                            .Serialize(netArgs.Encoding)));

                SendResources(netArgs);
            }
            else
            {
                netArgs.Send(
                    "upgrade-result".CreateCommand("-1"));
            }
            
            return result;
        }



        protected void SendResources(NetArgs netArgs)
        {
            netArgs.Connection.SendResources(netArgs.Connection.Account.Player);
        }
    }
}

