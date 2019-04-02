using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.DataAccess;

namespace LogUtility
{
    class ExceptionHandler : BaseExceptionHandler
    {
        static ExceptionHandler _Instance = new ExceptionHandler();

        public static ExceptionHandler Instance
        {
            get
            {
                return _Instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
        /// </summary>
        public ExceptionHandler()
            : base("ExceptionHandler")
        {
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected override void InnerHandleException(Exception ex)
        {
            if (ex == null || ex is System.Threading.ThreadAbortException)
                return;
            try
            {
                DefaultExceptionLog log = CreateExceptionLogEntity<DefaultExceptionLog>(ex);

                const string SQL_TEXT = @"INSERT INTO [TB_ExceptionLog]
                                        ([MachineName],[AssemblyName],[AppDomainName],[ThreadId],[WindowsIdentity],
                                        [EventID],[EventPriority],[EventSeverity],[CategoryName],[LogTime],[Title],
                                        [ExceptionSource],[ExceptionType],[HelpLink],[TargetSite],[Message],[FormattedMessage])
                                        VALUES(@MachineName, @AssemblyName, @AppDomainName, @ThreadId, @WindowsIdentity, 
                                        @EventID, @EventPriority, @EventSeverity, @CategoryName, @LogTime, @Title, @ExceptionSource, 
                                        @ExceptionType, @HelpLink, @TargetSite, @Message, @FormattedMessage)";
                DbAccessInformation accInfo = new DbAccessInformation(SQL_TEXT, System.Data.CommandType.Text);
                accInfo.AddParameter("@MachineName", log.MachineName);
                accInfo.AddParameter("@AssemblyName", log.AssemblyName);
                accInfo.AddParameter("@AppDomainName", log.AppDomainName);
                accInfo.AddParameter("@ThreadId", log.ThreadId);
                accInfo.AddParameter("@WindowsIdentity", log.WindowsIdentity);
                accInfo.AddParameter("@EventID", log.EventID);
                accInfo.AddParameter("@EventPriority", log.EventPriority);
                accInfo.AddParameter("@EventSeverity", log.EventSeverity);
                accInfo.AddParameter("@CategoryName", log.CategoryName);
                accInfo.AddParameter("@LogTime", log.LogTime);
                accInfo.AddParameter("@Title", log.Title);
                accInfo.AddParameter("@ExceptionSource", log.ExceptionSource);
                accInfo.AddParameter("@ExceptionType", log.ExceptionType);
                accInfo.AddParameter("@HelpLink", log.HelpLink);
                accInfo.AddParameter("@TargetSite", log.TargetSite);
                accInfo.AddParameter("@Message", log.Message);
                accInfo.AddParameter("@FormattedMessage", log.FormattedMessage);

                try
                {
                    using (SqlAccessCommand cmd = LogDbCommon.CreateSqlCommand())
                    {
                        cmd.AttachAccessInfo(accInfo);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    WriteEventLog(log);
                }

            }
            catch
            {
            }
        }
    }
}
