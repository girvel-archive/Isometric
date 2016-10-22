using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using BinarySerializationExtensions;
using CommandInterface;
using CommandInterface.Extensions;
using Isometric.CommonStructures;
using Isometric.Core.Modules;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Server.Modules.SpamModule;
using Newtonsoft.Json.Linq;
using SocketExtensions;
using _Connection = Isometric.Server.Connection;

namespace Isometric.Server.Modules.CommandModule.Server
{
    [Serializable]
    public class CommandManager
    {
        public Interface<NetArgs, CommandResult> Interface { get; set; }

        public Dictionary<Socket, int> SignupCodes { get; set; } = new Dictionary<Socket, int>();

        private readonly Isometric.Server.Server _server;



        public delegate void LoginEvent(string email, LoginResult result);

        public static event LoginEvent OnLoginAttempt;



        internal CommandManager(Isometric.Server.Server server)
        {
            _server = server;

            Interface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "login",
                    new[] {"account"},
                    _login),

                new Command<NetArgs, CommandResult>(
                    "email-send-code",
                    new[] {"email"},
                    _emailSendCode))
            {
                UnactiveCommands = new ICommand<NetArgs, CommandResult>[]
                {
                    new Command<NetArgs, CommandResult>(
                        "code-set",
                        new[] {"code"},
                        _codeSet),

                    new Command<NetArgs, CommandResult>(
                        "account-set",
                        new[] {"login", "account"},
                        _accountSet),
                }
            };
        }



        private CommandResult _login(Dictionary<string, string> args, NetArgs netArgs)
        {
            var receivedAccount = JObject.Parse(args["account"]);
            
            var suitableAccounts = netArgs.Server.Accounts.Where(
                a => a.Email == receivedAccount["Email"].ToString()
                     && a.Password == receivedAccount["Password"].ToString()).ToArray();

            var successful = suitableAccounts.Any();
            LoginResult result;

            if (successful)
            {
                if (suitableAccounts.First().Banned)
                {
                    result = LoginResult.Banned;
                    successful = false;
                }
                else
                {
                    result = LoginResult.Successful;
                }
            }
            else
            {
                result = LoginResult.Unsuccessful;
            }

            netArgs.Send("login-result".CreateCommand(((byte)result).ToString()));

            OnLoginAttempt?.Invoke(receivedAccount["Email"].ToString(), result);

            if (!successful)
            {
                return CommandResult.Unsuccessful;
            }

            var newConnection = new _Connection(
                netArgs.Socket, suitableAccounts.First(), netArgs.Server);

            netArgs.Server.CurrentConnections.Add(newConnection);

            newConnection.Start();

            return CommandResult.Successful;
        }

        // @email
        private CommandResult _emailSendCode(Dictionary<string, string> args, NetArgs netArgs)
        {
            var code = SingleRandom.Instance.Next(10000, 99999);

            SignupCodes[netArgs.Socket] = code;

            try
            {
                SmtpManager.Instance.SendSignupMail(args["email"], code);

                foreach (var command in new[] { "code-set", "account-set" })
                {
                    var i = 0;
                    Executor<CommandResult> result;

                    while (!Interface.TryGetExecutor(netArgs.ReceiveAll(), netArgs, out result, command)
                       || result() != CommandResult.Successful)
                    {
                        if (++i >= 3)
                        {
                            return CommandResult.Unsuccessful;
                        }
                        // TODO 1.x spam filter
                    }
                }

                netArgs.Socket.Close();
            }
            finally
            {
                SignupCodes.Remove(netArgs.Socket);
            }
            
            return CommandResult.Successful;
        }

        // @code
        private CommandResult _codeSet(Dictionary<string, string> args, NetArgs netArgs)
        {
            int code;
            if (!int.TryParse(args["code"], out code))
            {
                // TODO F Unity code-result
                netArgs.Send("code-result".CreateCommand(
                    ((byte)CodeResult.WrongCommand).ToString()));
                    
                return CommandResult.Spam;
            }

            if (code == SignupCodes[netArgs.Socket])
            {
                netArgs.Send("code-result".CreateCommand(
                    ((byte)CodeResult.Successful).ToString()));
                
                return CommandResult.Successful;
            }

            netArgs.Send("code-result".CreateCommand(((byte)CodeResult.WrongCode).ToString()));

            return CommandResult.Unsuccessful;
        }

        // @login,account
        private CommandResult _accountSet(Dictionary<string, string> args, NetArgs netArgs)
        {
            var account = args["account"].Deserialize<CommonAccount>(netArgs.Encoding);

            if (!Regex.IsMatch(args["login"], @"^[\w\s]*$"))
            {
                netArgs.Send("account-result".CreateCommand(
                    AccountCreatingResult.WrongLogin.ToString("d")));

                return CommandResult.Spam;
            }

            if (netArgs.Server.Accounts.Any(a => a.Email == account.Email))
            {
                netArgs.Send("account-result".CreateCommand(((byte)AccountCreatingResult.ExistingEmail).ToString()));

                return CommandResult.Unsuccessful;
            }

            if (netArgs.Server.Accounts.Any(a => a.Login == args["login"]))
            {
                netArgs.Send("account-result".CreateCommand(((byte)AccountCreatingResult.ExistingLogin).ToString()));

                return CommandResult.Unsuccessful;
            }

            var name = args["login"];
            netArgs.Server.Accounts.Add(new Account(name, account, new Player(name, _server.World, _server.PlayersManager)));
            netArgs.Send("account-result".CreateCommand(((byte)AccountCreatingResult.Successful).ToString()));

            return CommandResult.Successful;
        }
    }
}

