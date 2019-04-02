using Infrastructure;
using Opcomunity.Services;

namespace Order
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