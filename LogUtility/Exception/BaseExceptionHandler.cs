using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
// 在System.Configuration.Install中
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogUtility
{
    /// <summary>
    /// BaseExceptionHandler
    /// </summary>
    abstract class BaseExceptionHandler
    {
        /// <summary>
        /// LOG_NAME
        /// </summary>
        public const string LOG_NAME = "Application";

        /// <summary>
        /// 记录所有类型异常记录的跟踪信息
        /// </summary>
        protected static Dictionary<string, ExceptionHandlerTracer> ExceptionTable = new Dictionary<string, ExceptionHandlerTracer>();

        /// <summary>
        /// 相关配置
        /// </summary>
        protected Hashtable HandlerSettings = new Hashtable();

        /// <summary>
        /// 异常检测时间间隔，以分钟记
        /// </summary>
        protected int HandTimeSpan { get; set; }

        /// <summary>
        /// 每段时间写异常的最大次数
        /// </summary>
        protected int MaxHandTimes { get; set; }

        /// <summary>
        /// EventSource
        /// </summary>
        protected string EventSource { get; set; }

        /// <summary>
        /// Severity
        /// </summary>
        protected string EventSeverity { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        protected string CategoryName { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        protected string Title { get; set; }

        /// <summary>
        /// Reminder
        /// </summary>
        protected IExceptionHanderReminder Reminder { get; set; }

        /// <summary>
        /// _EventLog
        /// </summary>
        protected System.Diagnostics.EventLog _EventLog;

        /// <summary>
        /// Initializes the <see cref="BaseExceptionHandler"/> class.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        protected BaseExceptionHandler(string sectionName)
        {
            try
            {
                HandlerSettings = ConfigurationManager.GetSection(sectionName) as Hashtable;
            }
            catch
            { }

            if (HandlerSettings == null)
                HandlerSettings = new Hashtable();

            this.HandTimeSpan = TypeHelper.TryParse(HandlerSettings["HandTimeSpan"] as string, 1);
            this.MaxHandTimes = TypeHelper.TryParse(HandlerSettings["MaxHandTimes"] as string, 10);
            this.EventSource = TypeHelper.TryParse(HandlerSettings["EventSource"] as string, "EventSource");
            this.EventSeverity = TypeHelper.TryParse(HandlerSettings["Severity"] as string, "Error");
            this.CategoryName = TypeHelper.TryParse(HandlerSettings["Category"] as string, "Exception");
            this.Title = TypeHelper.TryParse(HandlerSettings["Title"] as string, "ExceptionHandler");
            //Reminder
            string typename = TypeHelper.TryParse(HandlerSettings["Reminder"] as string, string.Empty);
            Type type = TypeHelper.TryGetType(typename, null);
            if (type != null)
            {
                try
                {
                    this.Reminder = Activator.CreateInstance(type) as IExceptionHanderReminder;
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 创建EventLogInstaller
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public EventLogInstaller CreateEventLogInstaller(string source)
        {
            EventLogInstaller installer = new EventLogInstaller();
            installer.Log = LOG_NAME;
            installer.Source = source;
            return installer;
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public bool HandleException(Exception ex)
        {
            try
            {
                string key = GetExceptionKey(string.Empty, ex);
                ExceptionHandlerTracer tracer;
                if (!ExceptionTable.TryGetValue(key, out tracer))
                {
                    lock (ExceptionTable)
                    {
                        if (!ExceptionTable.TryGetValue(key, out tracer))
                        {
                            tracer = new ExceptionHandlerTracer();
                            ExceptionTable[key] = tracer;
                        }
                    }
                }

                //每段时间（HandTimeSpan）同一类异常只写MaxHandTimes次
                int timeCounter = (int)((DateTime.Now - tracer.BeginTime).TotalSeconds) / 60 / HandTimeSpan;
                int logCounter = tracer.LogCounter;
                if (timeCounter > 0)
                {
                    if (logCounter >= MaxHandTimes)
                    {
                        ex = new ErrorDuplicateException(ex, key, logCounter, logCounter - MaxHandTimes);
                        //报警                        
                        if (this.Reminder != null)
                        {
                            try
                            {
                                this.Reminder.Remind(ex);
                            }
                            catch
                            {
                            }
                        }
                    }
                    tracer.BeginTime = DateTime.Now;
                    System.Threading.Interlocked.Exchange(ref tracer.LogCounter, 1);
                    InnerHandleException(ex);
                    return true;
                }
                else
                {
                    System.Threading.Interlocked.Increment(ref tracer.LogCounter);
                    if (logCounter < MaxHandTimes)
                    {
                        InnerHandleException(ex);
                        return true;
                    }
                    else
                    {
                        //不写异常信息
                        return false;
                    }
                }
            }
            catch
            {
                //忽略此处异常
            }
            return false;
        }

        /// <summary>
        /// 写入系统日志
        /// </summary>
        /// <param name="log">The log.</param>
        public void WriteEventLog(IExceptionLog log)
        {
            try
            {
                if (_EventLog == null)
                {
                    lock (this)
                    {
                        if (_EventLog == null)
                            _EventLog = new System.Diagnostics.EventLog(LOG_NAME, log.MachineName, this.EventSource);
                    }
                }
                _EventLog.WriteEntry(log.FormattedMessage, System.Diagnostics.EventLogEntryType.Error, log.EventID);
            }
            catch
            {
                //忽略此处异常
            }
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected abstract void InnerHandleException(Exception ex);

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        protected T CreateExceptionLogEntity<T>(Exception ex) where T : IExceptionLog, new()
        {
            T log = new T();
            log.EventID = 100;
            log.EventPriority = 0;
            log.EventSeverity = this.EventSeverity;
            log.CategoryName = this.CategoryName;
            log.LogTime = DateTime.Now;
            log.Title = this.Title;
            log.ExceptionSource = TypeHelper.TryParse(ex.Source, string.Empty);
            log.ExceptionType = ex.GetType().FullName;
            log.HelpLink = TypeHelper.TryParse(ex.HelpLink, string.Empty);
            log.TargetSite = (ex.TargetSite == null ? string.Empty : TypeHelper.TryParse(ex.TargetSite.DeclaringType.FullName, string.Empty));
            log.Message = TypeHelper.GetSubString(ex.Message, 200);

            try
            {
                XmlExceptionFormatter formatter = null;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(ms, Encoding.UTF8);
                    formatter = new XmlExceptionFormatter(writer, ex);
                    // formatter.Format();
                    writer.Flush();
                    //log.FormattedMessage = System.Text.Encoding.Default.GetString(ms.ToArray());
                    log.FormattedMessage = TypeHelper.TryParse(ex.StackTrace.Trim(),"");
                }

                if (formatter != null)
                {
                    log.MachineName = TypeHelper.TryParse(formatter.AdditionalInfo["MachineName"], ".");
                    log.AssemblyName = TypeHelper.TryParse(formatter.AdditionalInfo["FullName"], string.Empty);
                    log.AppDomainName = TypeHelper.TryParse(formatter.AdditionalInfo["AppDomainName"], string.Empty);
                    log.ThreadId = TypeHelper.TryParse(Thread.CurrentThread.ManagedThreadId.ToString(), string.Empty);
                    log.WindowsIdentity = TypeHelper.TryParse(formatter.AdditionalInfo["WindowsIdentity"], string.Empty);
                }
            }
            catch
            {
            }

            return log;
        }

        /// <summary>
        /// 获取异常类型
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private string GetExceptionKey(string key, Exception ex)
        {
            if (ex == null)
                return key;
            else
                return GetExceptionKey(key + "_" + ex.GetType().FullName, ex.InnerException);
        }
    }

    /// <summary>
    /// 跟踪异常的类
    /// </summary>
    public class ExceptionHandlerTracer
    {
        /// <summary>
        /// 实际异常记录次数
        /// </summary>
        public int LogCounter = 0;

        /// <summary>
        /// 计数开始时间
        /// </summary>
        public DateTime BeginTime = DateTime.Now;
    }

    /// <summary>
    /// 重复异常
    /// </summary>
    public class ErrorDuplicateException : Exception
    {
        /// <summary>
        /// 重复异常消息
        /// </summary>
        protected static string ExceptionMessageFormat = "异常{0}产生了{1}次，忽略了{2}次。\r\n";

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDuplicateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ErrorDuplicateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDuplicateException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="args">The args.</param>
        public ErrorDuplicateException(Exception innerException, params object[] args)
            : base(string.Format(ExceptionMessageFormat, args), innerException)
        {
        }
    }

    /// <summary>
    /// IExceptionReminder
    /// </summary>
    public interface IExceptionHanderReminder
    {
        /// <summary>
        /// Reminds the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        void Remind(Exception ex);
    }
}
