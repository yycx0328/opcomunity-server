using Infrastructure;
using log4net;
using Mvc;
using Opcomunity.Services;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class DefaultController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        // GET: Default
        public JsonWebResult Index()
        { 
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }
                 
                var service = Ioc.Get<IDefaultService>();
                service.DataInitialize();
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark(); 
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult Config()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }
                string version = TypeHelper.TryParse(_requestParms.GetValue("version"), "");
                var configAppVersion = ConfigHelper.GetValue("AppVersion");
                int status = 1;
                if (!string.IsNullOrEmpty(version) && version == configAppVersion)
                    status = ConfigHelper.GetValue("AppOnlineStatus", 1);

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { Status = status };
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult CheckAppUpdate()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }
                int channel = TypeHelper.TryParse(_requestParms.GetValue("channel"), 0);
                string version = TypeHelper.TryParse(_requestParms.GetValue("version"), "");
                if (channel >= 4000)
                    channel = 4000;
                var service = Ioc.Get<IDefaultService>();
                var model = service.CheckAppUpdate(channel);
                if(model == null || (model!=null && string.Compare(version,model.Version) >= 0))
                { 
                    json.state = 1;
                    json.message = "已经是最新版本";
                    return ToJson(json);
                }
                bool isForceUpdate = false;
                if (!string.IsNullOrEmpty(model.MinVersion) && string.Compare(model.MinVersion, version) >= 0)
                    isForceUpdate = true;
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new {
                    IsForceUpdate = isForceUpdate,
                    Version = model.Version,
                    Title = model.Title,
                    Description = model.Description,
                    Link = model.Link
                };
                return ToJson(json);
            });
        }
        
        // GET: Default
        public JsonWebResult FeedBack()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                string description = TypeHelper.TryParse(_requestParms.GetValue("description"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(description))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if(description.Length>512)
                { 
                    json.state = 3000;
                    json.message = "内容最多500字！";
                    return ToJson(json);
                }

                var service = Ioc.Get<IDefaultService>();
                int status = service.SaveFeedBack(userId,token, description);
                if(status == 2)
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult GetTipOffCategory()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }
                  
                var service = Ioc.Get<IDefaultService>();
                var list = service.GetTipOffCategory(); 
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult TipOff()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                long anchorId = TypeHelper.TryParse(_requestParms.GetValue("anchorid"), 0L);
                int categoryId = TypeHelper.TryParse(_requestParms.GetValue("categoryid"), 0);
                string description = TypeHelper.TryParse(_requestParms.GetValue("description"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || anchorId<=0 
                    || categoryId<=0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if (description.Length > 512)
                {
                    json.state = 3000;
                    json.message = "内容最多500字！";
                    return ToJson(json);
                }

                var service = Ioc.Get<IDefaultService>();
                int status = service.SaveTipOff(userId, token,anchorId, categoryId, description);
                if (status == 2)
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult GetGifts()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IDefaultService>();
                var list = service.GetGifts();
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult GetMessage()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                int pageIndex = TypeHelper.TryParse(_requestParms.GetValue("pageindex"), 0);
                int pageSize = TypeHelper.TryParse(_requestParms.GetValue("pagesize"), 0);
                // 页码从1开始
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IDefaultService>();
                var list = service.GetMessage(userId, token, pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }
        
        public JsonWebResult InviteRewardRankList()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }
                 
                int pageIndex = TypeHelper.TryParse(_requestParms.GetValue("pageindex"), 0);
                int pageSize = TypeHelper.TryParse(_requestParms.GetValue("pagesize"), 0);
                var service = Ioc.Get<IDefaultService>();
                BasicTips tips = BasicTips.Init;
                var list = service.GetInviteRewardRankList(pageIndex, pageSize, out tips);
                if (tips != BasicTips.Success)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        public JsonWebResult ChannelStatistics()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = HttpContext.GetRequestParms();
                int channel = TypeHelper.TryParse(_requestParms.GetValue("channel"), 0);
                string start = TypeHelper.TryParse(_requestParms.GetValue("start"), "");
                string end = TypeHelper.TryParse(_requestParms.GetValue("end"), "");
                //Log4NetHelper.Info(log, channel + "  " + start + "  " + end);
                var SecurityIps = ConfigHelper.GetValue("SecurityIps");
                if (SecurityIps.ToLower() != "none" && SecurityIps.IndexOf(WebUtils.GetClientIP()) < 0)
                {
                    json.state = 2000;
                    json.message = "异常请求";
                    return ToJsonAllowGet(json);
                }

                DateTime startDate, endDate;
                if (channel <= 0 || !DateTime.TryParse(start, out startDate) || !DateTime.TryParse(end, out endDate))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJsonAllowGet(json);
                }

                var service = Ioc.Get<IDefaultService>();
                var list = service.GetChannelStatistics(channel, startDate, endDate);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJsonAllowGet(json);
            });
        }
    }
}