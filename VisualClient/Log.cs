using System;
using System.Collections.Generic;
using VisualServer;

namespace VisualClient
{
    [Serializable]
    public class Log : ILog
    {
        public List<LogMessage> Messages = new List<LogMessage>();
        public List<LogType> BlackList = new List<LogType>();
        
        public List<LogException> Exceptions = new List<LogException>();
        public List<Type> TypeBlackList = new List<Type>();



        public void Write(string message, LogType type)
        {
            Messages.Add(new LogMessage(DateTime.Now, message, type));

            if (!BlackList.Contains(type))
            {
                Console.WriteLine(message);
            }
        }
        
        public void Exception(Exception exception)
        {
            Exceptions.Add(new LogException(DateTime.Now, exception));

            if (!BlackList.Contains(LogType.Exception)
                && !TypeBlackList.Contains(exception.GetType()))
            {
                Console.WriteLine(exception.Message);
            }
        }

        public Log()
        {
        }

        public void CheckVersion(ProgramVersion version)
        {
            
        }
    }
}

