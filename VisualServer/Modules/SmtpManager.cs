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



        public delegate void MailSignupConstructor(string to, int code);

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

        public MailSignupConstructor ConstructMailSignup { get; set; }



        private static SmtpManager _constructDefault()
        {
            return new SmtpManager
            {
                Client = new SmtpClient {
                    // TODO default smtp data
                },

                ConstructMailSignup = (to, code) => {
                    return new MailMessage(
                        Instance.Client.Credentials.GetCredential().UserName,
                        to,
                        "Isometric game registration",
                        "Your code: " + code);
                },
            };
        }



        public static void SendSignupMail(string to, int code)
        {
            Instance.Client.Send(MailSignupConstructor(Instance.Client.Credentials.GetCredential()., to, code));
        }
    }
}

