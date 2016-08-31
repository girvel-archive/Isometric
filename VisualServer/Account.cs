using System;
using IsometricCore.Modules.PlayerModule;
using CommonStructures;

namespace VisualServer
{
    [Serializable]
    public class Account
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public AccountPermission Permission { get; set; }

        public DateTime BannedFrom { get; private set; }
        public TimeSpan BannedFor { get; private set; }

        public bool PermanentlyBanned { get; set; }

        public bool Banned 
            => PermanentlyBanned || BannedFrom + BannedFor > DateTime.Now;

        public int SpamErrorTimes { get; set; }

        public Player Player { get; set; }



        [Obsolete("using serialization ctor", true)]
        public Account() {}

        public Account(string login, string password, string email, AccountPermission permission)
        {
            Login = login;
            Password = password;
            Email = email;
            Permission = permission;

            Player = new Player(Login);
        }

        public Account(string login, CommonAccount common)
        {
            Login = login;
            Email = common.Email;
            Password = common.Password;
            Permission = AccountPermission.User;

            Player = new Player(Login);
        }



        public void Ban(TimeSpan @for)
        {
            BannedFrom = DateTime.Now;
            BannedFor = @for;
        }

        public override string ToString() => $"{Login}|{Password} :: {Permission}";
    }
}
