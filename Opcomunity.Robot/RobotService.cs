using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Robot
{
    public partial class RobotService : ServiceBase
    {
        public static readonly string SERVICE_NAME = "RobotService";
        private ServiceEngine engine = null;
        private static ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RobotService()
        {
            logger.Info("RobotService Initializing");
            this.CanHandlePowerEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.ServiceName = SERVICE_NAME;
            InitializeComponent();
            logger.Info("RobotService Initialize Completed");

        }

        protected override void OnStart(string[] args)
        {
            logger.Info("启动机器人");
            engine = new ServiceEngine();
            engine.Start();
            logger.Info("机器人启动完成");
        }

        protected override void OnStop()
        {
            logger.Info("停止机器人");
            if (engine != null)
            {
                engine.Stop();
            }
            logger.Info("机器人停止完成");
        }
    }
}
