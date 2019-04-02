using Infrastructure;
using log4net;
using Mvc;
using Newtonsoft.Json;
using Opcomunity.Data.Entities;
using Opcomunity.Services;
using Opcomunity.Services.Dtos;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Utility.Common;
using WebSite.Common;

namespace WebSite.Controllers
{
    public class VideoController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        //private static readonly string VideoFileBasePhysicalPath = ConfigHelper.GetValue("VideoFileBasePhysicalPath");
        //private static readonly string VideoFileWebRoot = ConfigurationManager.AppSettings["VideoFileWebRoot"];

        [HttpPost]
        public JsonWebResult Upload()
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
                Log4NetHelper.Info(log, "1、接收参数");
                HttpFileCollectionBase files = (HttpFileCollectionBase)Request.Files;
                Log4NetHelper.Info(log, "1.1、获取参数");
                if (files == null || files.Count <= 0
                    || files[0].ContentLength <= 0
                    || files[0].FileName.Length <= 0
                    || files[0].InputStream.Length <= 0)
                {
                    json.state = 2001;
                    json.message = "未获取到文件";
                    return ToJson(json);
                }

                Log4NetHelper.Info(log, "2、验证文件");
                var service = Ioc.Get<IVideoService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                string subDir = DateTime.Now.ToString("yyMM");
                string VideoFileBasePhysicalPath = ConfigHelper.GetValue("VideoFileBasePhysicalPath");
                string filePhysicalPath = string.Format("{0}/{1}", VideoFileBasePhysicalPath, subDir);

                HttpPostedFileBase file = files[0];
                string bucketSuffix = string.Format("show-{0}", DateTime.Now.ToString("yyMM"));
                string fileName = string.Format("Id{0}{1}", userId, DateTime.Now.ToString("ddHHmmss"));
                UploadVideoCheckResult checkResult = OSSFileCore.UploadVideo(file, filePhysicalPath, bucketSuffix, fileName);
                if (checkResult != UploadVideoCheckResult.Success)
                {
                    json.state = (int)checkResult;
                    json.message = checkResult.GetRemark();
                    return ToJson(json);
                }

                string imgExtension = ".jpg";
                string videoExtension = ".mp4";
                var videoLink = string.Format("http://{0}-{1}.{2}/{3}{4}", OSSConfig.VideoBucketPrefix, bucketSuffix, OSSConfig.Host, fileName, videoExtension);
                var imgPath = string.Format("http://{0}-{1}.{2}/{3}{4}", OSSConfig.ImageBucketPrefix, bucketSuffix, OSSConfig.Host, fileName, imgExtension);
                TB_UserVideo video = new TB_UserVideo()
                {
                    UserId = userId,
                    Extention = videoExtension,
                    Link = videoLink,
                    ImgPath = imgPath,
                    Description = description,
                    IsAvailable = true,
                    Views = 0,
                    Praises = 0,
                    IsFree = true,
                    Price = 0,
                    CreateTime = DateTime.Now
                };
                service.SaveUserVideo(video);

                json.state = (int)ValidateTips.Success;
                json.message = "上传成功";
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult List()
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
                int category = TypeHelper.TryParse(_requestParms.GetValue("category"), 0);
                int pageIndex = TypeHelper.TryParse(_requestParms.GetValue("pageindex"), 0);
                int pageSize = TypeHelper.TryParse(_requestParms.GetValue("pagesize"), 0);
                // 页码从1开始
                if (userId<=0 | string.IsNullOrEmpty(token) || pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                
                VideoListCategoryConfig config = VideoListCategoryConfig.Hottest;
                Enum.TryParse(category.ToString(), out config);
                var service = Ioc.Get<IVideoService>();
                if(!service.IsLoginUser(userId,token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }
                
                List<VideoItem> list = service.GetVideoList(userId,config, pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult Praise()
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
                long videoId = TypeHelper.TryParse(_requestParms.GetValue("videoid"), 0L);
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token) || videoId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IVideoService>();
                var tips = service.PraiseVideo(userId, token, videoId);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult Delete()
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
                long videoId = TypeHelper.TryParse(_requestParms.GetValue("videoid"), 0L);
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token) || videoId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IVideoService>();
                var tips = service.DeleteVideo(userId, token, videoId);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }
        
        // GET: Recommend
        public JsonWebResult Views()
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
                long videoId = TypeHelper.TryParse(_requestParms.GetValue("videoid"), 0L);
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token) || videoId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IVideoService>();
                var tips = service.ViewVideo(userId, token, videoId);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult MyVideoList()
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
                if (userId <= 0 || string.IsNullOrEmpty(token) || pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                
                var service = Ioc.Get<IVideoService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                List<VideoItem> list = service.GetMyVideoList(userId, pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult AnchorVideoList()
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
                int pageIndex = TypeHelper.TryParse(_requestParms.GetValue("pageindex"), 0);
                int pageSize = TypeHelper.TryParse(_requestParms.GetValue("pagesize"), 0);
                // 页码从1开始
                if (userId <= 0  || string.IsNullOrEmpty(token) || anchorId<=0 || pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IVideoService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                List<VideoItem> list = service.GetAnchorVideoList(userId, anchorId, pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }
    }
}