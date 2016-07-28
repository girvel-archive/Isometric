using System;
using GameBasics;

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

        /// <summary>
        /// Was last connection aborted because of spam
        /// </summary>
        public int SpamErrorTimes { get; set; }

        public Player Player { get; set; }
        // TODO normal territory generation


        public Account(string login, string password, string email, AccountPermission permission, World world, Game game)
        {
            Login = login;
            Password = password;
            Email = email;
            Permission = permission;

            Player = new Player(login, world, game); // TODO choosing world
        }

        public void Ban(TimeSpan @for)
        {
            BannedFrom = DateTime.Now;
            BannedFor = @for;
        }

        public override string ToString() => $"{Login}|{Password} :: {Permission}";
    }
}
