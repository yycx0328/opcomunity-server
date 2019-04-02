using Infrastructure;
using log4net;
using Mvc;
using Opcomunity.Data.Entities;
using Opcomunity.Services.Interface;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WebSite.Controllers
{
    public class QiniuController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public JsonWebResult GetUploadToken()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();

                #region 获取并验证参数
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                int price = TypeHelper.TryParse(_requestParms.GetValue("price"), 0);
                string description = TypeHelper.TryParse(_requestParms.GetValue("description"), "");
                 
                if (userId<=0 || price<=0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                #endregion
                QiniuHelper helper = new QiniuHelper();
                string token = helper.GetUploadToken();
                Log4NetHelper.Info(log, token);
                if (!string.IsNullOrEmpty(token))
                {
                    TB_Topic topic = new TB_Topic()
                    {
                        UserId = userId,
                        Price = price,
                        Description = description,
                        IsAvailable = true,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };
                    var service = Ioc.Get<ITopicService>();
                    long topicId = service.SaveTopic(topic);
                    if (topicId > 0)
                    {
                        json.state = (int)ValidateTips.Success;
                        json.message = ValidateTips.Success.GetRemark();
                        json.data = new { Token = token, TopicId = topicId };
                        return ToJson(json);
                    }
                    json.state = (int)ValidateTips.Faild;
                    json.message = "作品生成失败";
                    return ToJson(json);
                }
                json.state = (int)ValidateTips.Faild;
                json.message = "获取上传凭证失败";
                return ToJson(json);
            });
        }

        public JsonWebResult UploadCallback()
        {
            return Try(()=> {
                JsonBase json = new JsonBase();

                #region 回调鉴权
                string authorization = Request.Headers["Authorization"];
                Log4NetHelper.Info(log, authorization);
                string urlPath = Request.Url.AbsolutePath;
                Log4NetHelper.Info(log, urlPath);
                Stream reqStream = Request.InputStream;
                byte[] buffer = new byte[(int)reqStream.Length];
                reqStream.Read(buffer, 0, (int)reqStream.Length);
                string requestBody = buffer.ToString();
                Log4NetHelper.Info(log, requestBody);
                QiniuHelper qiniuHelper = new QiniuHelper();
                bool result = qiniuHelper.VerifyUploadCallback(authorization, urlPath, requestBody);
                #endregion

                QiniuCallbackBody body = JSONSerializeUtil.ToObject<QiniuCallbackBody>(requestBody);
                var service = Ioc.Get<ITopicService>();
                if (result)
                {
                    #region 上传成功后记录业务数据库中
                    TB_OssObject oss = new TB_OssObject()
                    {
                        TopicId = body.TopicId,
                        Bucket = body.Bucket,
                        OssKey = body.Key,
                        FileSize = body.FileSize,
                        HashValue = body.HashValue,
                        MimeType = body.MimeType,
                        Ext = body.Ext,
                        SortId = body.SortId,
                        IsLock = body.IsLock,
                        IsAvailable = true,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };
                    bool success = service.SaveTopicOssObject(oss); 
                    #endregion

                    if (success)
                    {
                        json.state = (int)ValidateTips.Success;
                        json.message = "上传成功";
                        return ToJson(json);
                    }
                }

                service.DeleteTopic(body.TopicId);
                json.state = (int)ValidateTips.Faild;
                json.message = "上传失败";
                return ToJson(json);
            });
        }
    }
}