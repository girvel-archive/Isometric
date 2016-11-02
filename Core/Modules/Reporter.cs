using System;

namespace Isometric.Core.Modules
{
    public class Reporter
    {
        private static Reporter _instance;
        public static Reporter Instance => _instance ?? (_instance = new Reporter());



        public event Action<string, Exception> OnError;



        public void ReportError(string message, Exception ex = null)
        {
            OnError?.Invoke(message, ex);
        }
    }
}