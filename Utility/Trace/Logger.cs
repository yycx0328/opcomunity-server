using System;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Logger
    /// </summary>
    public sealed class Logger
    {
        #region TraceInfo
        static string TraceClass = typeof(Logger).FullName;
        static bool IsTraced = Tracer.Instance.IsTraced(typeof(Logger));
        #endregion

        /// <summary>
        /// Instance
        /// </summary>
        public static Logger Instance
        {
            get 
            {
                return Singleton<Logger>.GetInstance();
            }
        }

        /// <summary>
        /// NewInstance
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="logName"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Logger NewInstance(string machineName, string logName, string source)
        {
            Logger logger = new Logger(); 
            logger.Initialize(machineName, logName, source);
            return logger;
        }

        /// <summary>
        /// LoggerProxy
        /// </summary>
        public DataProcessProxy<LogObject> LoggerProxy { get; set; }

        private Dictionary<string, LogCounter> LogCounterList;

        internal EventLog BaseLogger = null;

        /// <summary>
        /// According to documentation the size must be less than 16384 bytes
        /// </summary>
        public const int MaxEventLogStringSize = 16384 - 1;

        /// <summary>
        /// Logger
        /// </summary>
        public Logger()
        {            
            LogCounterList = new Dictionary<string, LogCounter>();
        }        

        /// <summary>
        /// HasInitialized
        /// </summary>
        public bool HasInitialized { get; private set; }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="logName"></param>
        /// <param name="source"></param>
        public void Initialize(string machineName, string logName, string source)
        {
            BaseLogger = new EventLog(logName, machineName, source);

            HasInitialized = true;
        }        

        /// <summary>
        /// Truncates the input text to be less than MaxEventLogStringSize if necessary.
        /// </summary>
        /// <param name="eventLogMessage">The string to truncate</param>
        /// <returns>A string guaranteed to be no longer than MaxEventLogStringSize</returns>
        private string TruncateForEventLog(string eventLogMessage)
        {
            //if the message is larger than max size truncate it
            //otherwise pass it straight out
            if (eventLogMessage != null && (eventLogMessage.Length > MaxEventLogStringSize))                
                return eventLogMessage.Substring(0, MaxEventLogStringSize);
            else                
                return eventLogMessage;            
        }

        private void LogWrite(LogObject entry)
        {            

            try
            {
                if (entry.EventId == int.MinValue)
                    BaseLogger.WriteEntry(entry.Message, entry.LogType);
                else
                    BaseLogger.WriteEntry(entry.Message, entry.LogType, entry.EventId);
            }
            catch
            {
                //ignore log exception
            }
        }

        private void BaseWriteEntry(string message, EventLogEntryType logType, int eventId)
        {
            if (HasInitialized)
            {
                var obj = new LogObject(message, logType, eventId);
                if (LoggerProxy == null)
                    LogWrite(obj);
                else
                    LoggerProxy.Process(obj);
            }
        }

        /// <summary>
        /// Provides message Event Log writing.
        /// </summary>        
        /// <param name="message">The messange to write to the log.</param>
        /// <param name="logType">The type of Event Log entry being written.</param>
        /// <param name="eventId">The error number to write; less than zero(0) if none</param>
        public void WriteEntry(string message, EventLogEntryType logType, int eventId)
        {
            message = TruncateForEventLog(message);

            BaseWriteEntry(message, logType, eventId);

            #region TraceInfo
            if (IsTraced)
            {
                Tracer.Instance.Write(TraceClass, "WriteEntry",
                    new string[] { "message", "logType", "eventId" },
                    new object[] { message, logType, eventId });
            }
            #endregion
        }

        /// <summary>
        /// Provides message Event Log writing.
        /// </summary>        
        /// <param name="message">The messange to write to the log.</param>
        /// <param name="logType">The type of Event Log entry being written.</param>        
        public void WriteEntry(string message, EventLogEntryType logType)
        {
            this.WriteEntry(message, logType, int.MinValue);
        }

        /// <summary>
        /// Provides message Event Log writing.
        /// </summary>        
        /// <param name="message">The messange to write to the log.</param>
        public void WriteEntry(string message)
        {
            this.WriteEntry(message, EventLogEntryType.Information, int.MinValue);
        }

        private LogCounter GetLogCounter(string key)
        {
            LogCounter counter;
            if (LogCounterList.TryGetValue(key, out counter))
                return counter;
            else
                return null;
        }
        
        /// <summary>
        /// Provides error handled Event Log writing.
        /// </summary>
        /// <param name="ex">The exception to write to the log</param>
        /// <param name="errorCode">The error code of the exception</param>
        public void WriteEntry(Exception ex, int errorCode)
        {
            string logKey = MakeLogKey(ex, errorCode);
            LogCounter counter = GetLogCounter(logKey);
            bool needRefresh = false;
            bool needWriteLog = true;
            lock (LogCounterList)
            {
                counter = GetLogCounter(logKey);
                if (counter == null)
                {
                    counter = new LogCounter();
                    LogCounterList.Add(logKey, counter);
                }
                else
                {
                    needRefresh = counter.NeedRefresh();
                    needWriteLog = counter.NeedWriteLog();
                    if (needRefresh)
                        counter.Refresh();
                    else
                        counter.Increase();
                }
            }

            if (needWriteLog)
            {
                string message = TruncateForEventLog(ex.ToString());
                BaseWriteEntry(message, EventLogEntryType.Error, errorCode);
            }
            else if (needRefresh)
            {
                string message = string.Format("{0} of {1} skipped.", counter.SkippedCounter, logKey);
                BaseWriteEntry(message, EventLogEntryType.Information, 0);
            }

            #region TraceInfo
            if (IsTraced)
            {
                Tracer.Instance.Write(TraceClass, "WriteEntry", ex, errorCode);
            }
            #endregion
        }

        /// <summary>
        /// Provides error handled Event Log writing.
        /// </summary>
        /// <param name="ex">The exception to write to the log</param>
        public void WriteEntry(Exception ex)
        {
            WriteEntry(ex, int.MinValue);
        }

        private string MakeLogKey(Exception ex, int errorCode)
        {
            StringBuilder sb = new StringBuilder();
            while (ex != null)
            {
                sb.Append(ex.GetType().FullName + ":");
                ex = ex.InnerException;
            }
            sb.Append(errorCode);
            return sb.ToString();
        }        

        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            BaseLogger.Dispose();
        }

        #endregion
    }
}
