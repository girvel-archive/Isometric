using System;
using System.Text;

namespace CommonStructures
{
    [Serializable]
    public class CommonAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }



        public CommonAccount(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Obsolete("using serialization ctor", true)]
        public CommonAccount() {}
    }
}