using System;
using System.Net;
using System.Net.Mail;
using Isometric.Server.Modules;

namespace Isometric.Client.Modules
{
    public class SingleMailManager : IMailManager
    {
        private static SingleMailManager _instance;
        public static SingleMailManager Instance => _instance ?? (_instance = new SingleMailManager());



        public string From { get; set; }



        [NonSerialized]
        private bool _connected = false;

        private SmtpClient _client;



        public void Send(string to, string subject, string message)
        {
            _client.Send(From, to, subject, message);
        }

        public void Connect(string host, int port, bool enableSsl, string email, string password)
        {
            _client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(email, password),
            };
            From = email;

            try
            {
                _client.Send(From, From, "testing server smtp", "test");
            }
            catch (SmtpException)
            {
                throw new ArgumentException("Something went wrong; SmtpException");
            }
            catch (FormatException)
            {
                throw new ArgumentException("Wrong email format");
            }

            _connected = true;
        }
    }
}