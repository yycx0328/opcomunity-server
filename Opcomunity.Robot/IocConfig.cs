using Infrastructure;
using Opcomunity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Robot
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
