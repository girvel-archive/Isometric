using System;
using VisualServer;

namespace VisualClient {
    [Serializable]
    public class LogMessage {
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public LogType Type { get; set; }

        public LogMessage(DateTime time, string message, LogType type) {
            Time = time;
            Message = message;
            Type = type;
        }

        public string FormatInfo => Time + " " + Type + " " + Message;
    }
}

