using System;
using Isometric.CommonStructures;
using Isometric.Core.Modules.PlayerModule;

namespace Isometric.Server
{
    [Serializable]
    public class Account
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Player Player { get; set; }



        /// <summary>
        /// Serialization ctor
        /// </summary>
        public Account() {}

        public Account(string login, string password, string email, Player player)
        {
            Login = login;
            Password = password;
            Email = email;

            Player = player;
        }

        public Account(string login, CommonAccount common, Player player)
        {
            Login = login;
            Email = common.Email;
            Password = common.Password;

            Player = player;
        }



        public override string ToString() => $"{typeof (Account).Name}; Login: {Login}, Password: {Password}";
    }
}
