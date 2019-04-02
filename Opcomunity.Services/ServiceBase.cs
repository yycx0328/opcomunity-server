using System;
using Infrastructure;
using Opcomunity.Data.Entities;
using log4net;
using System.Reflection;

namespace Opcomunity.Services
{
    public interface IServiceBase
    {
    }

    public abstract class ServiceBase
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        protected OpcomunityContext NewContext()
        {
            return new OpcomunityContext();
        }

        protected void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(log,ex);
            }
        }
    }
}
