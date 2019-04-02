using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Opcomunity.Robot
{
    static class Program
    {
        private static ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void RunService()
        {
            IocConfig.RegisterIoc();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RobotService()
            };
            ServiceBase.Run(ServicesToRun);
        }

        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                //如果没有参数则首先获取服务状态
                ServiceController sc = new ServiceController(RobotService.SERVICE_NAME);
                try
                {
                    //如果服务存在且不是运行状态，启动服务
                    if (!sc.Status.Equals(ServiceControllerStatus.Running))
                    {
                        RunService();
                    }
                }
                catch
                {
                    //如果有错误，可能是服务没有安装，则自动安装服务
                    try
                    {
                        SelfInstaller.InstallMe();
                        return;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            else
            {
                //如果有参数，则根据参数决定执行的操作
                if (args.Length == 1 && args[0].Length > 1
                    && (args[0][0] == '-' || args[0][0] == '/'))
                {
                    switch (args[0].Substring(1).ToLower())
                    {
                        default:
                            break;
                        case "install":
                        case "i":
                            SelfInstaller.InstallMe();
                            break;
                        case "uninstall":
                        case "u":
                            SelfInstaller.UninstallMe();
                            break;
                    }
                }
                else
                {
                    RunService();
                }
            }
        }
    }
}
