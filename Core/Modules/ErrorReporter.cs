using System;

namespace Isometric.Core.Modules
{
    public class ErrorReporter
    {
        private static ErrorReporter _instance;
        public static ErrorReporter Instance => _instance ?? (_instance = new ErrorReporter());



        public event Action<string> OnError;



        public void ReportError(string message)
        {
            OnError?.Invoke(message);
        }
    }
}