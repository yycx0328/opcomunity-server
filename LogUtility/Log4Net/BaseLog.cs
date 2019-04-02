using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

public class BaseLog<T> where T : class, new()
{
    protected static ILog log = LogManager.GetLogger(typeof(T).Name);

    protected static void Logger(string function, Action tryHandle, Action<Exception> catchHandle = null, Action finnalyHandle = null)
    {
        Log4NetHelper.Logger(log, function, ErrorHandle.Throw, tryHandle, catchHandle, finnalyHandle);
    }
}