using Infrastructure;
using log4net;
using Opcomunity.Data.Entities;
using Opcomunity.Services.Interface;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebSite
{
    public class QiniuHelper
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        public bool VerifyUploadCallback(string authorization, string urlPath, string requestBody)
        {
            try
            {
                string[] authArray = authorization.Split(' ')[1].Split(':');
                string accessKey = authArray[0];
                string encoded_data = authArray[1];
                Log4NetHelper.Info(log, string.Format("accessKey={0}    encoded_data={1}", accessKey, encoded_data));
                if (accessKey == QiniuConfig.AccessKey)
                {
                    string data = string.Format("{0}\n{1}", urlPath, requestBody);
                    string sign_data = Util.HmacSha1(QiniuConfig.SecretKey, data);
                    Log4NetHelper.Info(log, string.Format("data={0}    sign_data={1}", data, sign_data));
                    if (encoded_data.Equals(sign_data))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
                return false;
            }
        }

        public string GetUploadToken()
        {
            var service = Ioc.Get<IQiniuService>();
            string token = service.GetUploadToken();
            if (!string.IsNullOrEmpty(token))
                return token;
            
            string qiniuAccessKey = QiniuConfig.AccessKey;
            string qiniuSecretKey = QiniuConfig.SecretKey;
            string bucket = QiniuConfig.Bucket;
            int tokenExpiresSecond = QiniuConfig.TokenExpiresSecond;
            Mac mac = new Mac(qiniuAccessKey, qiniuSecretKey);
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(tokenExpiresSecond);
            putPolicy.CallbackUrl = QiniuConfig.UploadCallbackUrl;
            putPolicy.CallbackBody = "{\"Key\":\"$(key)\",\"HashValue\":\"$(etag)\",\"FileSize\":$(fsize),\"Bucket\":\"$(bucket)\",\"MimeType\":\"$(mimeType)\",\"Ext\":\"$(ext)\",\"TopicId\":$(x:topicid),\"SortId\":$(x:sortid),\"IsLock\":$(x:islock)}";
            putPolicy.CallbackBodyType = "application/json";
            string newToken = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            if (!string.IsNullOrEmpty(newToken))
            {
                TB_QiniuUploadToken model = new TB_QiniuUploadToken()
                {
                    Token = newToken,
                    ExpiresTime = DateTime.Now.AddSeconds(tokenExpiresSecond - 60),
                    CreateTime = DateTime.Now
                };
                service.SaveUploadToken(model);
                return newToken;
            }

            return string.Empty;
        }
    }
}