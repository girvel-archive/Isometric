using System;
using CommandInterface;
using VisualServer.Modules.SpamModule;
using System.Collections.Generic;
using CommonStructures;
using IsometricCore.Modules.WorldModule.Buildings;
using VisualServer.Extensions;
using IsometricCore.Modules;
using BinarySerializationExtensions;
using VectorNet;
using System.Linq;
using SocketExtensions;

namespace VisualServer.Modules.CommandModule.Connection
{
    public class CommandManager
    {
        #region Singleton-part

        [Obsolete("using backing field")]
        private static CommandManager _instance;

        #pragma warning disable 618

        public static CommandManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommandManager()._setDefault();
                }

                return _instance;
            }

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



        public Interface<NetArgs, CommandResult> Interface { get; set; }



        private CommandManager _setDefault()
        {
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

            return this;
        }



        private CommandResult _sendResources(
                Dictionary<string, string> args, NetArgs netArgs)
        {
            netArgs.Connection.SendResources(netArgs.Connection.Account.Player);

            return CommandResult.Successful;
        }

        private CommandResult _getTerritory(
                Dictionary<string, string> args, NetArgs netArgs)
        {
            // FIXME unity st@size,buildings -> set-territory@common_territory
            netArgs.Send("set-territory".CreateCommand(
                netArgs.Connection.Account.Player.Territory.ToCommon().SerializeToString(
                    netArgs.Connection.Encoding)));

            return CommandResult.Successful;
        }

        // @building
        private CommandResult _getBuildingContextActions(
                Dictionary<string, string> args, NetArgs netArgs)
        {
            var position = netArgs.Connection.Encoding.GetBytes(args["building"])
                .ByteDeserialize<IntVector>();
            var building = netArgs.Connection.Account.Player.Territory[position];
            var pattern = building.Pattern;
            var patternNodes = BuildingGraph.Instance.Find(pattern);

            if (patternNodes.Any())
            {
                // FIXME Unity sba -> set-building-actions
                netArgs.Send("set-building-actions".CreateCommand(
                    patternNodes[0].Children.Select(
                        c => new CommonBuildingAction(
                            netArgs.Connection.Account.Player.CurrentResources.Enough(c.Value.NeedResources) 
                                && c.Value.UpgradePossible(pattern, building),
                            $"Upgrade to {c.Value.Name}",
                            new CommonBuilding(position),
                            pattern.ID))
                    .SerializeToString(netArgs.Connection.Encoding)));
            }

            return CommandResult.Successful;
        }

        // @action
        private CommandResult _useBuildingContextAction(
                Dictionary<string, string> args, NetArgs netArgs)
        {
            var action = netArgs.Connection.Encoding.GetBytes(args["message"])
                .ByteDeserialize<CommonBuildingAction>();
            var subject = netArgs.Connection.Account.Player.Territory[action.Subject.Position];
            var upgrade = BuildingPattern.Find(action.Upgrade);

            return subject.TryUpgrade(upgrade) 
                ? CommandResult.Successful 
                : CommandResult.Unsuccessful;
        }
    }
}

