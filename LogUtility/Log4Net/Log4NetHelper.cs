using System;
using System.IO;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
public class Log4NetHelper
{
    public static void InitLog4net()
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory + "log4net.config";
        FileInfo fileInfo = new FileInfo(filePath);
        log4net.Config.XmlConfigurator.Configure(fileInfo);
    }

    #region 利用Action委托封装LOG4NET对方法的处理
    /// <summary>
    /// 利用Action委托封装LOG4NET对方法的处理
    /// </summary>
    /// <param name="log">日志对象</param>
    /// <param name="function">方法名</param>
    /// <param name="errorHandle">异常处理方法</param>
    /// <param name="tryHandle">调试/运行代码</param>
    /// <param name="catchHandle">异常处理方式</param>
    /// <param name="finnalyHandle">最终处理方式</param>
    public static void Logger(ILog log, string function, ErrorHandle errorHandle, Action tryHandle, Action<Exception> catchHandle = null,
        Action finnalyHandle = null)
    {
        try
        {
            log.Info(function);
            tryHandle();
        }
        catch (Exception ex)
        {
            log.Error(function + " Error", ex);
            if (catchHandle != null)
                catchHandle(ex);
            if (errorHandle == ErrorHandle.Throw)
                throw ex;
        }
        finally
        {
            if (finnalyHandle != null)
                finnalyHandle();
        }
    }
    #endregion

    public static void Info(ILog log, object message)
    {
        log.Info(message);
    }

    public static void Error(ILog log, object message)
    {
        log.Error(message);
    }
}