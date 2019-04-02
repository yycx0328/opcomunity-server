using System;
using Infrastructure;
using Opcomunity.Passport.Entities;

namespace Passport.Services
{
    public interface IServiceBase
    {
    }

    public abstract class ServiceBase
    {
        protected PassportContext NewContext()
        {
            return new PassportContext();
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
