using System;

namespace VisualServer
{
    public interface ILog
    {
        void Exception(Exception e);
        void Write(string message, LogType type);
    }
}