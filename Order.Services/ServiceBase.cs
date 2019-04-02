using System;
using Infrastructure;
using Opcomunity.Data.Entities;

namespace Order.Services
{
    public interface IServiceBase
    {
    }

    public abstract class ServiceBase
    {
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
                LogHelper.TryLog("ServiceBase.Try", ex);
            }
        }
    }
}
