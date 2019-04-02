using Infrastructure;
using log4net;
using Mvc;
using Passport.Common;
using Passport.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Passport.Controllers
{
    public class AccountController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        #region 用户注册：验证用户提交验证码是否有效及设置密码，验证码有效，且用户不存在，且密码符合规则
        /// <summary>
        /// 用户注册：验证用户提交验证码是否有效及设置密码，验证码有效，且用户不存在，且密码符合规则
        /// </summary>
        /// <returns></returns>
        public JsonWebResult IsPhoneRegist()
        {
            return Try(() => {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                string phoneNo = TypeHelper.TryParse(_requestParms.GetValue("phoneno"), "");
                string applicationId = TypeHelper.TryParse(_requestParms.GetValue("applicationid"), "");
                if (string.IsNullOrEmpty(phoneNo))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if (!StringHelper.TryRegex(phoneNo, RegularType.Mobile))
                {
                    json.state = -2000;
                    json.message = "请输入有效的手机号码！";
                    return ToJson(json);
                }
                
                var service = Ioc.Get<IAccountService>();
                var result = service.IsPhoneRegist(phoneNo);
                if (result)
                {
                    json.state = -2000;
                    json.message = "该手机号已注册";
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                return ToJson(json);
            });
        }
        #endregion
    }
}