using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommandInterface.Extensions;
using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Game.Modules;
using Isometric.Server;
using Isometric.Server.Extensions;
using Isometric.Server.Modules;
using Isometric.Vector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Isometric.Client.Modules
{
    public class SingleRequestManager : IRequestManager
    {
        private static SingleRequestManager _instance;
        public static SingleRequestManager Instance => _instance ?? (_instance = new SingleRequestManager());



        private delegate bool Request(JObject request, Connection connection);

        private readonly Dictionary<string, Request> _commands;



        public delegate void LoginEvent(string email, LoginResult result);

        public static event LoginEvent OnLoginAttempt;



        public SingleRequestManager()
        {
            _commands = new Dictionary<string, Request>
            {
                ["Get resources"] = _sendResources,
                ["Get area"] = _getArea,
                ["Get building actions"] = _getBuildingContextActions,
                ["Upgrade building"] = _upgrade,
                ["Sign in"] = _login,
                ["Sign up"] = _accountSet,
            };
        }



        public bool Execute(JObject request, Connection connection)
        {
            try
            {
                return _commands[request["Request"].ToString()](request, connection);
            }
            catch
            {
                return false;
            }
        }



        private bool _login(JObject request, Connection connection)
        {
            var args = request["Args"];

            var account = connection.Server.Accounts
                .FirstOrDefault(a => a.Email == args["Email"].ToString()
                                     && a.Password == args["Password"].ToString());

            var successful = account != null;

            var result = successful ? LoginResult.Successful : LoginResult.Unsuccessful;

            connection.Send("login-result".CreateCommand(((byte)result).ToString()));

            OnLoginAttempt?.Invoke(args["Email"].ToString(), result);

            if (!successful)
            {
                return false;
            }

            connection.Account = account;

            return true;
        }


        private bool _accountSet(JObject request, Connection connection)
        {
            var args = request["Args"];
            string message;
            var success = false;

            if (!Regex.IsMatch(args["Login"].ToString(), @"^[\w ]*$"))
            {
                message = "Incorrect login";
            }
            else if (connection.Server.Accounts.Any(a => a.Login == args["Login"].ToString()))
            {
                message = "Existing login";
            }
            else if (connection.Server.Accounts.Any(a => a.Email == args["Email"].ToString()))
            {
                message = "Existing email";
            }
            else
            {
                message = "Success";
                success = true;
            }

            connection.Send(
                new JObject
                {
                    ["Args"] = new JObject
                    {
                        ["Result"] = message,
                    }
                }.ToString());

            return success;
        }


        private bool _sendResources(JObject request, Connection connection)
        {
            SendResources(connection);
            return true;
        }


        private bool _getArea(JObject request, Connection connection)
        {
            connection.Send(
                "set-territory".CreateCommand(
                    connection.Account.Player.Area.ToJson().ToString()));

            return true;
        }


        private bool _getBuildingContextActions(JObject request, Connection connection)
        {
            var position = request["Args"]["Position"].ToObject<IntVector>();

            var building = connection.Account.Player.Area[position];

            var pattern = building.Pattern;
            var patternNode = SingleBuildingGraph.Instance.FirstOrDefault(node => node.Value == pattern);

            if (patternNode != null)
            {
                connection.Send(
                    "set-building-actions".CreateCommand(
                        CommonHelper.CreateContextActionsData(
                            children: patternNode.GetChildren(),
                            possibleSelector: c => c.Value.UpgradePossible(
                                connection.Account.Player.CurrentResources,
                                pattern,
                                building),
                            textSelector: c => $"Upgrade to {c.Value.Name}",
                            idSelector: c => c.Value.Id
                            ).ToString()));

            }

            return true;
        }


        private bool _upgrade(JObject request, Connection connection)
        {
            var action = request["Args"];

            var subject =
                connection.Account.Player.Area[
                    JsonConvert.DeserializeObject<IntVector>(action["Position"].ToString())];
            var upgrade = BuildingPattern.Find(int.Parse(action["Upgrade to"].ToString()));

            var result = subject.TryUpgrade(upgrade, connection.Account.Player);

            if (result)
            {
                connection.Send(
                    "upgrade-result".CreateCommand(
                        CommonHelper.CreateUpgradeData(upgrade.Id, subject.Position).ToString()));

                SendResources(connection);
            }
            else
            {
                connection.Send("upgrade-result".CreateCommand("-1"));
            }

            return result;
        }



        protected void SendResources(Connection connection)
        {
            connection.SendResources(connection.Account.Player);
        }
    }
}