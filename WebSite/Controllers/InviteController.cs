using Infrastructure;
using log4net;
using Mvc;
using Newtonsoft.Json.Linq;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class InviteController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        public JsonWebResult GetShareUserInfo()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = HttpContext.GetRequestParms();
                long userId = TypeHelper.TryParse(_requestParms.GetValue("vid"), 0L);
                if (userId<=0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJsonAllowGet(json);
                }
                
                var service = Ioc.Get<ILoginService>();
                var user = service.GetUserById(userId);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { Avatar = user.ThumbnailAvatar };
                return ToJsonAllowGet(json);
            });
        }

        public JsonWebResult GetPhoneVerifyCode()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = HttpContext.GetRequestParms();
                string phoneNo = TypeHelper.TryParse(_requestParms.GetValue("phoneno"), "");
                if (string.IsNullOrEmpty(phoneNo))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJsonAllowGet(json);
                }

                if (!StringHelper.TryRegex(phoneNo, RegularType.Mobile))
                {
                    json.state = -2000;
                    json.message = "请输入有效的手机号码！";
                    return ToJsonAllowGet(json);
                }
                 
                var service = Ioc.Get<ILoginService>();
                if(service.IsPhoneRegistOrInvited(phoneNo))
                {
                    json.state = 2000;
                    json.message = "该手机号已经注册或被邀请过";
                    return ToJsonAllowGet(json);
                }

                string messageId = service.SendMessageCode(phoneNo);
                if (string.IsNullOrEmpty(messageId))
                {
                    json.state = 2000;
                    json.message = "短信验证码发送失败";
                    return ToJsonAllowGet(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { MessageId = messageId };
                return ToJsonAllowGet(json);
            });
        }

        public JsonWebResult Send()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = HttpContext.GetRequestParms();
                long userId = TypeHelper.TryParse(_requestParms.GetValue("vid"), 0L);
                string phoneNo = TypeHelper.TryParse(_requestParms.GetValue("phoneno"), "");
                string messageId = TypeHelper.TryParse(_requestParms.GetValue("messageid"), "");
                string code = TypeHelper.TryParse(_requestParms.GetValue("code"), "");
                if (userId<=0 || string.IsNullOrEmpty(phoneNo) 
                    || string.IsNullOrEmpty(messageId) || string.IsNullOrEmpty(code))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJsonAllowGet(json);
                }

                if (!StringHelper.TryRegex(phoneNo, RegularType.Mobile))
                {
                    json.state = -2000;
                    json.message = "请输入有效的手机号码！";
                    return ToJsonAllowGet(json);
                }

                var service = Ioc.Get<ILoginService>();
                string errMessage = "";
                if (!service.ValidMessageCode(phoneNo, messageId, code, out errMessage))
                {
                    json.state = 2000;
                    json.message = errMessage;
                    return ToJsonAllowGet(json);
                }

                var inviteTips = service.SaveInviteData(userId, phoneNo); 
                json.state = (int)inviteTips;
                json.message = inviteTips.GetRemark(); 
                return ToJsonAllowGet(json);
            });
        }
        
        //public JsonWebResult RunNeteaseAccount()
        //{
        //    return Try(() =>
        //    {
        //        JsonBase json = new JsonBase(); 
        //        var service = Ioc.Get<ICoinService>();
        //        service.ModifyNetease();
        //        json.state = (int)ValidateTips.Success;
        //        json.message = ValidateTips.Success.GetRemark();
        //        return ToJsonAllowGet(json);
        //    });
        //}
    }
}