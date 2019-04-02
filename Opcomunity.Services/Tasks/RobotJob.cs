using Infrastructure;
using log4net;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Tasks
{
    class RobotJob : IJob
    {
        private static ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Task IJob.Execute(IJobExecutionContext context)
        {
            var executeTime = DateTime.Now;
            var service = Ioc.Get<IRobotService>();
            var list = service.GetWaitingSendList(executeTime);
            if(list!=null)
            {
                foreach(var message in list)
                {
                    NameValueCollection data = new NameValueCollection();
                    data.Add("from", message.FromAccId);
                    data.Add("ope", "0");
                    data.Add("to", message.ToAccId);
                    data.Add("type", message.Type.ToString());
                    data.Add("body", message.Body);
                    string result = NeteaseCore.PostNeteaseRequest(NeteaseRequestActionConfig.SEND_MSG, data);
                    logger.Info("Id:"+ message.Id + "    result:" + result);
                }
            }
            return Task.FromResult(true);
        }
    }
}
