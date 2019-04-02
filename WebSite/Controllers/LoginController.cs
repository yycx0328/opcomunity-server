using Common;
using Infrastructure;
using log4net;
using Mvc;
using Opcomunity.Data.Entities;
using Opcomunity.Services;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using System.Collections.Generic;
using System.Reflection;

namespace WebSite.Controllers
{
    public class LoginController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        public JsonWebResult GetPhoneVerifyCode()
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

                string phoneNo = TypeHelper.TryParse(_requestParms.GetValue("phoneno"), "");
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

                var service = Ioc.Get<ILoginService>();
                string messageId = service.SendMessageCode(phoneNo); 
                if (string.IsNullOrEmpty(messageId))
                {
                    json.state = 2000;
                    json.message = "短信验证码发送失败！";
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { MessageId = messageId };
                return ToJson(json);
            });
        }

        public JsonWebResult PhoneRegist()
        {
            return Try(()=> { 
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                string phoneNo = TypeHelper.TryParse(_requestParms.GetValue("phoneno"), "");
                string messageId = TypeHelper.TryParse(_requestParms.GetValue("messageid"), "");
                string code = TypeHelper.TryParse(_requestParms.GetValue("code"), "");
                string digest = TypeHelper.TryParse(_requestParms.GetValue("digest"), "");
                int channel = TypeHelper.TryParse(_requestParms.GetValue("channel"), 0);
                if (string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(digest)
                    || string.IsNullOrEmpty(messageId) || string.IsNullOrEmpty(code))
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

                var service = Ioc.Get<ILoginService>();
                string errMessage = "";
                if (!service.ValidMessageCode(phoneNo, messageId, code, out errMessage))
                {
                    json.state = 2000;
                    json.message = errMessage;
                    return ToJson(json);
                }

                string applicationId = ConfigHelper.GetValue("PassportApplicationId");
                string desKey = ConfigHelper.GetValue("3DESKEY");
                string desIV = ConfigHelper.GetValue("3DESIV");

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
                string nickName = Tools.GetDefaultNickName("MLQ-", 8);
                string avatar = Tools.GetDefaultAvatar();
                string thubnail = Tools.GetDefaultThumbnailAvatar();
                CheckResultTips resultTips = CheckResultTips.InitErr;
                long userId = service.PhoneRegist(phoneNo, password, applicationId, nickName, avatar, thubnail, channel, out resultTips);
                if (userId <=0)
                {
                    json.state = (int)resultTips;
                    json.message = resultTips.GetRemark();
                    return ToJson(json);
                }
                // 注册网易云账号
                var neteaseAccount = service.GetNeteaseAccountIfNotExistThenRegist(userId, nickName, avatar);
                // 获取返回用户信息
                var model = service.GetLoginUserInfo(userId);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = model;

                var accountService = Ioc.Get<IAccountService>();
                accountService.SaveWaitingSendMessage(userId);
                return ToJson(json);
            });
        }

        public JsonWebResult PhoneLogin()
        {
            return Try(() => {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
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

                string desKey = ConfigHelper.GetValue("3DESKEY");
                string desIV = ConfigHelper.GetValue("3DESIV");
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
                var user = service.GetPhoneLoginUserId(phoneNo, password);
                if (user == null)
                {
                    json.state = (int)CheckResultTips.LoginAccountOrPasswordErr;
                    json.message = CheckResultTips.LoginAccountOrPasswordErr.GetRemark();
                    return ToJson(json);
                }
                // 更新Token
                service.UpdateUserToken(user.Id);
                // 注册网易云账号
                var neteaseAccount = service.GetNeteaseAccountIfNotExistThenRegist(user.Id, user.NickName, user.Avatar);
                // 获取返回用户信息
                var model = service.GetLoginUserInfo(user.Id);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = model;
                return ToJson(json);
            });
        }

        public JsonWebResult PhoneRestPassword()
        {
            return Try(() => {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                string phoneNo = TypeHelper.TryParse(_requestParms.GetValue("phoneno"), "");
                string messageId = TypeHelper.TryParse(_requestParms.GetValue("messageid"), "");
                string code = TypeHelper.TryParse(_requestParms.GetValue("code"), "");
                string digest = TypeHelper.TryParse(_requestParms.GetValue("digest"), "");
                if (string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(code) 
                    || string.IsNullOrEmpty(digest) || string.IsNullOrEmpty(messageId))
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

                var service = Ioc.Get<ILoginService>();
                string errMessage = "";
                if (!service.ValidMessageCode(phoneNo, messageId, code, out errMessage))
                {
                    json.state = 2000;
                    json.message = errMessage;
                    return ToJson(json);
                }

                string desKey = ConfigHelper.GetValue("3DESKEY");
                string desIV = ConfigHelper.GetValue("3DESIV");

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
                CheckResultTips resultTips = CheckResultTips.InitErr;
                var userId = service.ResetPassword(phoneNo, password, out resultTips);
                if (userId <= 0)
                {
                    json.state = (int)resultTips;
                    json.message = resultTips.GetRemark();
                    return ToJson(json);
                } 
                var user = service.GetUserById(userId);
                if (user == null)    // 保存用户基本信息
                { 
                    json.state = (int)2000;
                    json.message = "获取用户信息失败";
                    return ToJson(json);
                }
                // 更新用户token
                service.UpdateUserToken(userId);
                // 注册网易云账号
                var neteaseAccount = service.GetNeteaseAccountIfNotExistThenRegist(user.Id, user.NickName, user.Avatar);
                // 获取返回用户信息 
                var model = service.GetLoginUserInfo(user.Id);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = model;
                return ToJson(json);
            });
        }

        /// <summary>
        /// 登录网易云账号
        /// </summary>
        /// <returns></returns>
        public JsonWebResult LoginToNetease()
        {
            return Try(() => {
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
                if (userId<=0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<ILoginService>();
                var userInfo = service.GetLoginUserInfo(userId, token);
                if (userInfo == null)
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                TB_NeteaseAccount netAccount = service.GetNeteaseAccountIfNotExistThenRegist(userInfo.Id, userInfo.NickName, userInfo.Avatar);
                // 如果该账号已经注册好了网易云账号，则直接返回给客户端用于登录
                if (netAccount == null)
                {
                    json.state = 2000;
                    json.message = "非获取到网易云通讯账号！";
                    return ToJson(json);  
                }

                json.state = (int)ValidateTips.Success;
                json.message = "登录成功";
                json.data = new
                {
                    netAccount.UserId,
                    netAccount.LoginStatus,
                    netAccount.ChatStatus,
                    netAccount.NeteaseAccId,
                    netAccount.NeteaseToken
                };
                return ToJson(json);
            });
        }

        public JsonWebResult VisitorLogin()
        {
            return Try(() => {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                string uuid = TypeHelper.TryParse(_requestParms.GetValue("uuid"), "");
                int channel = TypeHelper.TryParse(_requestParms.GetValue("channel"), 0);
                if (string.IsNullOrEmpty(uuid))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                
                var service = Ioc.Get<ILoginService>();
                string applicationId = ConfigHelper.GetValue("PassportApplicationId");
                string nickName = Tools.GetDefaultNickName("游客-", 8);
                string avatar = Tools.GetDefaultAvatar();
                string thubnail = Tools.GetDefaultThumbnailAvatar();
                CheckResultTips resultTips = CheckResultTips.InitErr;
                long userId = service.VisitorLogin(uuid, applicationId, nickName, avatar, thubnail, channel, out resultTips);
                if (userId <= 0)
                {
                    json.state = (int)resultTips;
                    json.message = resultTips.GetRemark();
                    return ToJson(json);
                }
                // 注册网易云账号
                var neteaseAccount = service.GetNeteaseAccountIfNotExistThenRegist(userId, nickName, avatar);
                // 获取返回用户信息
                var model = service.GetLoginUserInfo(userId);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = model;

                var accountService = Ioc.Get<IAccountService>();
                accountService.SaveWaitingSendMessage(userId);

                return ToJson(json);
            });
        }

    }
}
