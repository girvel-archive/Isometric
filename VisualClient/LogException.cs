using System;

namespace VisualClient {
    [Serializable]
    public class LogException {
        public DateTime Time { get; }
        public Exception DetectedException { get; }

        public LogException(DateTime time, Exception detectedException) {
            Time = time;
            DetectedException = detectedException;
        } 
    }
}