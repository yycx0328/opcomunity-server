using System;
using System.Diagnostics;

namespace Utility
{
    /// <summary>
    /// LogObject
    /// </summary>
    [Serializable]    
    public sealed class LogObject
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// LogType
        /// </summary>
        public EventLogEntryType LogType { get; set; }

        /// <summary>
        /// EventId
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// LogObject
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="eventId"></param>
        public LogObject(string message, EventLogEntryType logType, int eventId)
        {
            this.Message = message;
            this.LogType = logType;
            this.EventId = eventId;
        }
    }

    /// <summary>
    /// This class is designed to avoid flood the event log
    /// </summary>
    internal class LogCounter
    {
        public static int CountLimit = 100;
        public static int MinuteLimit = 10;

        public DateTime StartTime = DateTime.Now;
        public int Counter = 1;

        public void Refresh()
        {
            StartTime = DateTime.Now;
            Counter = 1;
        }

        public void Increase()
        {
            Counter++;
        }

        public bool NeedRefresh()
        {
            return ((DateTime.Now - StartTime).TotalSeconds > MinuteLimit * 60);
        }

        public bool NeedWriteLog()
        {
            return (Counter < CountLimit);
        }

        public int SkippedCounter
        {
            get { return Counter - CountLimit; }
        }
    }
}
