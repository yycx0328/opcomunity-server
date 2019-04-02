using Infrastructure;
using Passport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passport
{
    public class IocConfig
    {
        public static void RegisterIoc()
        {
            Ioc.RegisterInheritedTypes(typeof(ServiceBase).Assembly, typeof(ServiceBase));
            Ioc.Register<ILogger, FileLogger>();
        }
    }
}