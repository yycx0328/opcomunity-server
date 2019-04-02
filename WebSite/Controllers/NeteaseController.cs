using Infrastructure;
using log4net;
using Opcomunity.Services;
using Opcomunity.Services.Interface;
using System;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Utility.Common;

namespace WebSite.Controllers
{
    public class NeteaseController : Controller
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        [HttpPost]
        public void Notify()
        {
            Log4NetHelper.Info(log, "Notify Execute!");
            using (StreamReader sr = new StreamReader(Request.InputStream))  
            {
                try
                {
                    string curTime = TypeHelper.TryParse(Request.Headers["CurTime"], "");
                    string md5 = TypeHelper.TryParse(Request.Headers["MD5"], "");
                    string checkSum = TypeHelper.TryParse(Request.Headers["CheckSum"], "");
                    var requestBody = sr.ReadLine();
                    if (requestBody.Replace(" ", "") != "{}")
                    {
                        Log4NetHelper.Info(log, "=============回调开始=============");
                        Log4NetHelper.Info(log, "Netease Request Body:" + requestBody);
                        var service = Ioc.Get<INeteaseService>();
                        NeteaseCallNotifyTips tips = service.SaveLiveCallRecord(md5, curTime, checkSum, requestBody);
                        Log4NetHelper.Info(log, "NeteaseCallNotifyTips:" + tips.GetRemark());
                        Log4NetHelper.Info(log, "=============回调结束=============");
                        if ((int)tips >= 2000)
                            Response.StatusCode = 201;
                    }
                }catch(Exception ex)
                {
                    Response.StatusCode = 201;
                    ExceptionLogHelper.Instance.WriteExceptionLog(ex);
                }
            }
        }
    }
}