using System;
using System.Net.Mail;

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
            get {
                if (_instance == null)
                {
                    _instance = _constructDefault();
                }

                return _instance;
            }

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
        public static SmtpClient SingleClient
        {
            get
            {
                return Instance.Client;
            }

            set
            {
                Instance.Client = value;
            }
        }

        public string From { get; set; }

        public MailSignupConstructor ConstructMailSignup { get; set; }



        private static SmtpManager _constructDefault()
        {
            return new SmtpManager
            {
                Client = new SmtpClient {
                    // FIXME default smtp data
                },

                ConstructMailSignup = (to, code) => {
                    return new MailMessage(
                        Instance.From,
                        to,
                        "Isometric game registration",
                        "Your code: " + code);
                },
            };
        }



        public static void SendSignupMail(string to, int code)
        {
            Instance.Client.Send(Instance.ConstructMailSignup(to, code));
        }
    }
}

