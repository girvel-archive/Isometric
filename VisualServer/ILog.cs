using System;

namespace VisualServer
{
    public interface ILog
    {
        void Exception(Exception e, bool user);
        void Write(string message, LogType type);
    }
}