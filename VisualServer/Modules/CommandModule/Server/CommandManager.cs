using System;
using VisualServer.Modules.SpamModule;
using CommandInterface;
using System.Collections.Generic;
using System.IO;
using CommonStructures;
using BinarySerializationExtensions;
using IsometricCore.Modules;
using System.Linq;
using _Connection = VisualServer.Connection;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using CommandInterface.Extensions;
using SocketExtensions;

namespace VisualServer.Modules.CommandModule.Server
{
    [Serializable]
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



        public Interface<NetArgs, CommandResult> Interface { get; set; }

        public Dictionary<Socket, int> SignupCodes { get; set; }



        public delegate void LoginEvent(string email, LoginResult result);

        public event LoginEvent OnLoginAttempt;



        private CommandResult _login(Dictionary<string, string> args, NetArgs netArgs)
        {
            CommonAccount receivedAccount;

            using (var stream = new MemoryStream(netArgs.Encoding.GetBytes(args["account"])))
            {
                receivedAccount = (CommonAccount)new BinaryFormatter().Deserialize(stream);
            }
            
            var suitableAccounts = netArgs.Server.Accounts.Where(
                a => a.Email == receivedAccount.Email
                     && a.Password == receivedAccount.Password).ToArray();

            Console.WriteLine(suitableAccounts.Any());

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

            OnLoginAttempt?.Invoke(receivedAccount.Email, result);

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
        private CommandResult _codeSet(
            Dictionary<string, string> args, NetArgs netArgs)
        {
            int code;
            if (!int.TryParse(args["code"], out code))
            {
                // FIXME Unity code-result
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
            var account = netArgs.Deserialize<CommonAccount>(args["account"]);

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

            netArgs.Server.Accounts.Add(new Account(args["login"], account));
            netArgs.Send("account-result".CreateCommand(((byte)AccountCreatingResult.Successful).ToString()));

            return CommandResult.Successful;
        }



        private CommandManager _setDefault()
        {
            Interface = new Interface<NetArgs, CommandResult>(
                new Command<NetArgs, CommandResult>(
                    "login",
                    new[] { "account" },
                    _login),

                new Command<NetArgs, CommandResult>( // FIXME Unity email-send-code
                    "email-send-code",
                    new[] { "email" },
                    _emailSendCode))
            {
                UnactiveCommands = new Command<NetArgs, CommandResult>[]
                {
                    new Command<NetArgs, CommandResult>(
                        "code-set",
                        new[] { "code" },
                        _codeSet),

                    new Command<NetArgs, CommandResult>(
                        "account-set",
                        new[] { "login", "account" },
                        _accountSet),
                }
            };

            return this;
        }
    }
}

