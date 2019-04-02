using Common;
using Infrastructure;
using log4net;
using Mvc;
using Passport.Common;
using Passport.Services.Interface;
using System.Collections.Generic;
using System.Reflection;
using Utility.Common;

namespace Passport.Controllers
{
    public class LoginController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        #region 用户注册：验证用户提交验证码是否有效及设置密码，验证码有效，且用户不存在，且密码符合规则
        /// <summary>
        /// 用户注册：验证用户提交验证码是否有效及设置密码，验证码有效，且用户不存在，且密码符合规则
        /// </summary>
        /// <returns></returns>
        public JsonWebResult PhoneRegist()
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
                string digest = TypeHelper.TryParse(_requestParms.GetValue("digest"), "");
                if (string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(digest))
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

                string desKey = System.Configuration.ConfigurationManager.AppSettings["3DESKEY"];
                string desIV = System.Configuration.ConfigurationManager.AppSettings["3DESIV"];
                
                // 将客户端传递的密码进行3DE解密
                string password = SecurityHelper.TripleDESDecrypst(digest,desKey,desIV);
                if (string.IsNullOrEmpty(password))
                {
                    json.state = (int)CheckResultTips.UnrecognizedPasswordErr;
                    json.message = CheckResultTips.UnrecognizedPasswordErr.GetRemark();
                    return ToJson(json);
                }
                // 重新加密密码用作DB存储
                password = WebUtils.MD5(string.Format("{0}{1}{2}",desIV,password, desKey));

                var service = Ioc.Get<ILoginService>();
                CheckResultTips tips = CheckResultTips.InitErr;
                var userId = service.PhoneRegistUserIfNotExist(phoneNo, password, applicationId, out tips);
                if (userId <= 0)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    return ToJson(json);
                }
                
                json.state = (int)ValidateTips.Success;
                json.message = "注册成功";
                json.data = userId;
                return ToJson(json);
            });
        }
        #endregion

        #region 用户登录：账号密码登录
        /// <summary>
        /// 验证用户提交验证码是否有效，有效则返回登录成功信息，否则返回失败提示
        /// </summary>
        /// <returns></returns>
        public JsonWebResult PhoneLogin()
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
                string digest = TypeHelper.TryParse(_requestParms.GetValue("digest"), "");
                if (string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(digest))
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

                string desKey = System.Configuration.ConfigurationManager.AppSettings["3DESKEY"];
                string desIV = System.Configuration.ConfigurationManager.AppSettings["3DESIV"];

                // 将客户端传递的密码进行3DE解密
                string password = SecurityHelper.TripleDESDecrypst(digest, desKey, desIV);
                if (string.IsNullOrEmpty(password))
                {
                    json.state = (int)CheckResultTips.UnrecognizedPasswordErr;
                    json.message = CheckResultTips.UnrecognizedPasswordErr.GetRemark();
                    return ToJson(json);
                }
                // 重新加密密码用作DB存储
                password = WebUtils.MD5(string.Format("{0}{1}{2}", desIV, password, desKey));

                var service = Ioc.Get<ILoginService>();
                var userId = service.GetPhoneLoginUserId(phoneNo, password);
                if (userId <= 0)
                {
                    json.state = (int)CheckResultTips.LoginAccountOrPasswordErr;
                    json.message = CheckResultTips.LoginAccountOrPasswordErr.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = "登录成功";
                json.data = userId;
                return ToJson(json);
            });
        }
        #endregion

        #region 重置密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public JsonWebResult PhoneRestPassword()
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
                string digest = TypeHelper.TryParse(_requestParms.GetValue("digest"), "");
                if (string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(digest))
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

                string desKey = System.Configuration.ConfigurationManager.AppSettings["3DESKEY"];
                string desIV = System.Configuration.ConfigurationManager.AppSettings["3DESIV"];

                // 将客户端传递的密码进行3DE解密
                string password = SecurityHelper.TripleDESDecrypst(digest, desKey, desIV);
                if (string.IsNullOrEmpty(password))
                {
                    json.state = (int)CheckResultTips.UnrecognizedPasswordErr;
                    json.message = CheckResultTips.UnrecognizedPasswordErr.GetRemark();
                    return ToJson(json);
                }
                // 重新加密密码用作DB存储
                password = WebUtils.MD5(string.Format("{0}{1}{2}", desIV, password, desKey));

                var service = Ioc.Get<ILoginService>();
                CheckResultTips tips = CheckResultTips.InitErr;
                var userId = service.ResetPassword(phoneNo, password, out tips);
                if (userId<=0)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = "重置密码成功";
                json.data = userId;

                return ToJson(json);
            });
        }
        #endregion
    }
}
