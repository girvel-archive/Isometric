using System;
using System.Text;
using System.IO;
using BinarySerializationExtensions;

namespace VisualClient.Modules.LogModule
{
    [Serializable]
    public class Log
    {
        #region Singleton

        [Obsolete("using backing field")]
        private static Log _instance;

        #pragma warning disable 0618

        public static Log Instance {
            get { return _instance ?? (_instance = new Log()); }

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

        #endregion



        static Log()
        {
            
        }

        private Log() 
        {
            if (!Directory.Exists(FileFolder))
            {
                Directory.CreateDirectory(FileFolder);
            }

            _currentPath = $"{FileFolder}/{DateTime.Now:yy-MM-dd}.log";

            LogEvents.Init();
        }



        public Encoding Encoding { get; set; } = Encoding.ASCII;

        public const string FileFolder = "logs";

        private readonly string _currentPath;



        public void Write(string message)
        {
            message = DateTime.Now.ToString("yy.MM.dd hh:mm:ss") + '\t' + message;

            using (var writer = new StreamWriter(_currentPath, true, Encoding))
            {
                writer.WriteLine(message);
            }

            Console.WriteLine(message);
        }



        public void Exception(Exception exception, string message = "")
        {
            Write($"{exception} caught.\n\tMessage: \"{message}\"\n\t" +
                "Serialized exception:\n" 
                + Console.OutputEncoding.GetString(exception.SerializeToBytes()));
        }
    }
}

