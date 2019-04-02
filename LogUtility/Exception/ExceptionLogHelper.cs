using LogUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExceptionLogHelper
{
    private ExceptionLogHelper() { }
    public static ExceptionLogHelper Instance = new ExceptionLogHelper();
    
    #region 异常日志
    public void WriteExceptionLog(Exception ex)
    {
        ExceptionHandler.Instance.HandleException(ex);
    }
    #endregion
}
