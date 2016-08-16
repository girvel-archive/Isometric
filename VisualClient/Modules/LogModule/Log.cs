using System;
using System.Collections.Generic;
using VisualServer;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            get {
                if (_instance == null)
                {
                    _instance = new Log();
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

            _currentPath = $"{FileFolder}/{DateTime.Now}";

            LogEvents.Init();
        }



        public Encoding Encoding { get; set; } = Encoding.ASCII;

        public const string FileFolder = "logs";

        private string _currentPath;



        public void Write(string message)
        {
            message = DateTime.Now.ToString("yy.MM.dd hh:mm:ss") + '\t' + message;

            using (var writer = new StreamWriter(_currentPath, true, Encoding))
            {
                writer.WriteLine(message);
            }

            Console.WriteLine(message);
        }

        public void CheckVersion()
        {

        }



        public void Exception(Exception exception)
        {
            Write(exception.ToString() + " caught. Serialized exception:");

            // TODO log using stream -> stream field
            using (var stream = File.Open(_currentPath, FileMode.Append))
            {
                new BinaryFormatter().Serialize(stream, exception);
            }
        }
    }
}

