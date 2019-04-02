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
    public class CollectController : JsonController
    {
        public JsonWebResult GetCollectTopics()
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

                string strUserId = TypeHelper.TryParse(_requestParms.GetValue("userid"), "");
                long userId = 0;
                if (string.IsNullOrEmpty(strUserId) || !long.TryParse(strUserId, out userId))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<ICollectService>();
                List<TopicItem> data = service.GetCollectTopics(userId);
                if (data == null || data.Count == 0)
                    data = null;
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = data;
                return ToJson(json);
            });
        }

        public JsonWebResult CollectTopic()
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

                string strUserId = TypeHelper.TryParse(_requestParms.GetValue("userid"), "");
                string strTopicId = TypeHelper.TryParse(_requestParms.GetValue("topicid"), "");
                long userId = 0;
                long topicId = 0;
                if (string.IsNullOrEmpty(strUserId) || !long.TryParse(strUserId, out userId)
                    || string.IsNullOrEmpty(strTopicId) || !long.TryParse(strTopicId, out topicId))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<ICollectService>();
                CollectTips tips = service.CollectTopic(userId, topicId);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        public JsonWebResult CancelCollect()
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

                string strUserId = TypeHelper.TryParse(_requestParms.GetValue("userid"), "");
                string strTopicId = TypeHelper.TryParse(_requestParms.GetValue("topicid"), "");
                long userId = 0;
                long topicId = 0;
                if (string.IsNullOrEmpty(strUserId) || !long.TryParse(strUserId, out userId)
                    || string.IsNullOrEmpty(strTopicId) || !long.TryParse(strTopicId, out topicId))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<ICollectService>();
                CollectTips tips = service.CancelCollect(userId, topicId);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }
    }
}