using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Isometric.Client.Modules.LogModule
{
    [Serializable]
    public class Log
    {
        #region Singleton

        [Obsolete("using backing field")]
        private static Log _instance;

        #pragma warning disable 0618

        public static Log Instance => _instance ?? (_instance = new Log());

        #pragma warning restore 0618

        #endregion



        private Log() 
        {
            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);
            }

            if (!Directory.Exists(ExceptionsFolder))
            {
                Directory.CreateDirectory(ExceptionsFolder);
            }

            if (File.Exists(LogSettingsFile))
            {
                using (var stream = File.OpenRead(LogSettingsFile))
                using (var reader = new StreamReader(stream))
                {
                    SessionNo = (short)(short.Parse(reader.ReadToEnd()) + 1);
                }
            }

            using (var stream = File.OpenWrite(LogSettingsFile))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(SessionNo);
            }

            _currentPath = $"{LogFolder}/{DateTime.Now:yy-MM-dd}.log";
        }



        public Encoding Encoding { get; set; } = Encoding.ASCII;

        public short SessionNo { get; }

        public int ExceptionNo { get; set; }

        private readonly string _currentPath;



        private const string LogFolder = "logs";

        private const string ExceptionsFolder = LogFolder + "/exceptions";

        private const string LogSettingsFile = LogFolder + ".log-settings";



        public void Write(string message)
        {
            message = FormatMessage(DateTime.Now, message);

            const int iMax = 128;
            var i = 0;
            trying:
            try
            {
                using (var writer = new StreamWriter(_currentPath, true, Encoding))
                {
                    writer.WriteLine(message);
                }
            }
            catch (IOException)
            {
                if (++i < iMax)
                {
                    goto trying;
                }

                Console.WriteLine(FormatMessage(DateTime.Now, "Can't write to log file"));
            }

            Console.WriteLine(message);
        }



        public void Exception(Exception exception, string message = "")
        {
            var fileName = $"{ExceptionsFolder}/Exception {SessionNo}-{ExceptionNo++}";

            Write($"{exception.GetType()} was caught.\n" +
                  $"    Message: \"{message}\"\n" +
                  $"    Exception serialized as: {fileName}");

            using (var stream = File.OpenWrite(fileName))
            {
                new BinaryFormatter().Serialize(stream, exception);
            }
        }



        private static string FormatMessage(DateTime dateTime, string message)
        {
            return dateTime.ToString("yy.MM.dd hh:mm:ss") + '\t' + message;
        }
    }
}

