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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketExtensions;
using Isometric.Vector;

namespace Isometric.Server.Modules.CommandModule.Connection
{
    public class CommandManager
    {
        public Interface<NetArgs, CommandResult> CommandInterface { get; set; }

        private readonly Isometric.Server.Connection _connection;



        public CommandManager(Isometric.Server.Connection connection)
        {
            _connection = connection;

            CommandInterface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "get-territory", new string[0],
                    _getArea),

                new Command<NetArgs, CommandResult>(
                    "get-building-actions", new[] {"building"},
                    _getBuildingContextActions),

                new Command<NetArgs, CommandResult>(
                    "upgrade", new[] {"action"},
                    _upgrade),

                new Command<NetArgs, CommandResult>(
                    "get-resources", new string[0],
                    _sendResources));
        }



        private CommandResult _sendResources(Dictionary<string, string> args, NetArgs netArgs)
        {
            SendResources(netArgs);
            return CommandResult.Successful;
        }

        
        private CommandResult _getArea(Dictionary<string, string> args, NetArgs netArgs)
        {
            netArgs.Send(
                "set-territory".CreateCommand(
                    netArgs.Connection.Account.Player.Area.ToCommon()));

            return CommandResult.Successful;
        }


        // @building
        private CommandResult _getBuildingContextActions(Dictionary<string, string> args, NetArgs netArgs)
        {
            var commonBuilding = args["building"]
                .Deserialize<CommonBuilding>(netArgs.Connection.Encoding);

            var building = netArgs.Connection.Account.Player.Area[commonBuilding.Position];

            var pattern = building.Pattern;
            var patternNode = _connection.ParentServer.Graph.FirstOrDefault(node => node.Value == pattern);

            if (patternNode != null)
            {
                netArgs.Send(
                    "set-building-actions".CreateCommand(
                        patternNode
                            .GetChildren()
                            .Select(
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
        private CommandResult _upgrade(Dictionary<string, string> args, NetArgs netArgs)
        {
            var action = JObject.Parse(args["action"]);

            var subject =
                netArgs.Connection.Account.Player.Area[
                    JsonConvert.DeserializeObject<IntVector>(action["Position"].ToString())];
            var upgrade = BuildingPattern.Find(int.Parse(action["Upgrade to"].ToString()));

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
                netArgs.Send("upgrade-result".CreateCommand("-1"));
            }
            
            return result;
        }



        protected void SendResources(NetArgs netArgs)
        {
            netArgs.Connection.SendResources(netArgs.Connection.Account.Player);
        }
    }
}

