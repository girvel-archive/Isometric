using System;
using System.Text;
using CommandInterface;

namespace CompressedStructures
{
    [Serializable]
    public class CommonAccount : ICompressable
    {
        public string Email { get; set; }
        public string Password { get; set; }



        public byte[] GetBytes // TODO to argumenttype
            => Encoding.ASCII.GetBytes(GetString);

        public string GetString => $"[{Email};{Password}]";



        public CommonAccount(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public CommonAccount() {}



        public static CommonAccount GetFromBytes(byte[] bytes)
            => GetFromString(Encoding.ASCII.GetString(bytes));

        public static CommonAccount GetFromString(string @string)
        {
            var splitedString = @string.ParseType("[email;password]");
            return new CommonAccount(splitedString["email"], splitedString["password"]);
        }
    }
}