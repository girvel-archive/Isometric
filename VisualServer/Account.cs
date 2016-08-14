using System;
using GameCore.Modules.PlayerModule;

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

        public bool Banned {
            get {
                if (BannedFor == TimeSpan.Zero)
                {
                    return false;
                }

                if (BannedFrom + BannedFor < DateTime.Now)
                {
                    BannedFor = TimeSpan.Zero;
                    return PermanentlyBanned;
                }

                return true;
            }
        }

        public int SpamErrorTimes { get; set; }

        public Player Player { get; set; }



        public Account(string login, string password, string email, AccountPermission permission)
        {
            Login = login;
            Password = password;
            Email = email;
            Permission = permission;

            Player = new Player(login); // TODO choosing world
        }



        public void Ban(TimeSpan @for)
        {
            BannedFrom = DateTime.Now;
            BannedFor = @for;
        }

        public override string ToString() => $"{Login}|{Password} :: {Permission}";
    }
}
