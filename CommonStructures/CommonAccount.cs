using System;

namespace Isometric.CommonStructures
{
    [Serializable]
    public class CommonAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }



        public CommonAccount(string email, string password, int id)
        {
            Email = email;
            Password = password;
            Id = id;
        }

        [Obsolete("using serialization ctor", true)]
        public CommonAccount() {}
    }
}