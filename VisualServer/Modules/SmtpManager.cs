using System;
using System.Net.Mail;
using System.Net;

namespace VisualServer.Modules
{
    [Serializable]
    public class SmtpManager
    {
        #region Singleton

        [Obsolete("using backing field")]
        private static SmtpManager _instance;

        #pragma warning disable 0618

        public static SmtpManager Instance {
            get { return _instance ?? (_instance = _constructDefault()); }

            set {
        #if DEBUG
                if (_instance != null)
                {
                    throw new ArgumentException("Instance is already set");
                }
        #endif

                _instance = value;
            }
        }

        #pragma warning restore 0618

        public SmtpManager() {}

        #endregion



        public delegate MailMessage MailSignupConstructor(string to, int code);

        public SmtpClient Client { get; set; }

        public string From { get; set; }

        public MailSignupConstructor ConstructMailSignup { get; set; }



        [NonSerialized] private bool _connected = false;



        private static SmtpManager _constructDefault()
        {
            return new SmtpManager
            {
                ConstructMailSignup = (to, code) => {
                    return new MailMessage(
                        Instance.From,
                        to,
                        "Isometric game registration",
                        "Your code: " + code);
                },
            };
        }



        public void SendSignupMail(string to, int code)
        {
            #if DEBUG
            
            if (!_connected)
            {
                throw new NotImplementedException("Attempt to send mail before TryConnect()");
            }

            #endif

            Instance.Client.Send(Instance.ConstructMailSignup(to, code));
        }

        public void Connect(string host, int port, bool enableSsl, string email, string password)
        {
            Client = new SmtpClient(host, port)
                {
                    EnableSsl = enableSsl,
                    Credentials = new NetworkCredential(email, password),
                };
            From = email;

            try
            {
                Client.Send(From, From, "testing server smtp", "test");
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

