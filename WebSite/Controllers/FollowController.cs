using Infrastructure;
using Mvc;
using Opcomunity.Services;
using Opcomunity.Services.Dtos;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class FollowController : JsonController
    {
        public JsonWebResult GetFollowUsers()
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

                if (userId<=0 || string.IsNullOrEmpty(token) || pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<IFollowService>();
                if(!service.IsLoginUser(userId, token))
                { 
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }
                List<FollowUserItem> data = service.GetFollowUsers(userId, pageIndex, pageSize);
                if (data == null || data.Count == 0)
                    data = null;
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = data;
                return ToJson(json);
            });
        }
         
        public JsonWebResult FollowUser()
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
                long followedUserId = TypeHelper.TryParse(_requestParms.GetValue("followeduserid"), 0L);
                if (userId<=0 || string.IsNullOrEmpty(token) || followedUserId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<IFollowService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }
                FollowTips tips = service.FollowUser(userId, followedUserId); 
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }
         
        public JsonWebResult CancelFollow()
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
                long followedUserId = TypeHelper.TryParse(_requestParms.GetValue("followeduserid"), 0L);
                if (userId <= 0 || string.IsNullOrEmpty(token) || followedUserId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<IFollowService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }
                FollowTips tips = service.CancelFollow(userId, followedUserId);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        public JsonWebResult GetFollowMineUsers()
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

                if (userId <= 0 || string.IsNullOrEmpty(token) || pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<IFollowService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }
                List<FollowUserItem> data = service.GetFollowMineUsers(userId, pageIndex, pageSize);
                if (data == null || data.Count == 0)
                    data = null;
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = data;
                return ToJson(json);
            });
        }
    }
}