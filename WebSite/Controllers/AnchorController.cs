using Infrastructure;
using Mvc;
using Opcomunity.Data.Entities;
using Opcomunity.Services;
using Opcomunity.Services.Dtos;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utility.Common;
using WebSite.Common;

namespace WebSite.Controllers
{
    public class AnchorController : JsonController
    {
        //private static readonly string StaticFileBasePhysicalPath = ConfigurationManager.AppSettings["StaticFileBasePhysicalPath"];
        //private static readonly string StaticFileWebRoot = ConfigurationManager.AppSettings["StaticFileWebRoot"];

        // GET: Recommend
        public JsonWebResult GetBannerList()
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
                var service = Ioc.Get<IAnchorService>();
                List<BannerItem> list = service.GetBannerList(5);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GetRecommendAnchorList()
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
                // 页码从1开始
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<IAnchorService>();
                List<AnchorItem> list = service.GetRecormmendAnchorList(pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GetAnchorCategoryList()
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
                
                var service = Ioc.Get<IAnchorService>();
                var list = service.GetAnchorCategoryList();
                if(list == null || list.Count == 0)
                {
                    json.state = 2000;
                    json.message = "未获取到分类";
                }
                else
                {
                    json.state = (int)ValidateTips.Success;
                    json.message = ValidateTips.Success.GetRemark();
                    json.data = from c in list
                                select new {
                                    c.Id,
                                    c.Name,
                                    c.SortId
                                };
                }
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GetDiscoverAnchorList()
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

                int category = TypeHelper.TryParse(_requestParms.GetValue("category"), 0);
                int pageIndex = TypeHelper.TryParse(_requestParms.GetValue("pageindex"), 0);
                int pageSize = TypeHelper.TryParse(_requestParms.GetValue("pagesize"), 0);
                // 页码从1开始
                if (category<=0 || pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                var service = Ioc.Get<IAnchorService>();
                List<AnchorItem> list = service.GetDiscoverAnchorList(category, pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GetAnchorDetailInfo()
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

                // 页码从1开始
                if (anchorId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAnchorService>(); 
                var detailInfo = service.GetAnchorDetailInfo(userId, token, anchorId);

                if (detailInfo == null)
                {
                    json.state = 2000;
                    json.message = "未获取到主播详情！";
                    return ToJson(json);
                }
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = detailInfo;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult ApplyTobeAnchor()
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
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAnchorService>();
                var userInfo = service.GetLoginUserInfo(userId, token);
                if(userInfo == null)
                {
                    json.state = 2001;
                    json.message = "用户验证失败！";
                    return ToJson(json);
                }
                var anchor = new TB_Anchor
                {
                    UserId = userInfo.Id, 
                    CallRatio = 0,
                    CashRatio = 0,
                    Glamour = 0,
                    ApplyTime = DateTime.Now,
                    IsAuth = false,
                    IsBlack = false
                };
                AplayTobeAnchorTips tips = service.SaveAnchor(anchor);
                if(tips != AplayTobeAnchorTips.Success)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    return ToJson(json);
                }
                
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult UploadAnchorIdentity()
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
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAnchorService>();
                if (!service.IsAnchorExist(userId,token))
                {
                    json.state = 2001;
                    json.message = "用户验证失败！";
                    return ToJson(json);
                }

                HttpFileCollectionBase files = (HttpFileCollectionBase)Request.Files;
                if (files == null || files.Count <= 0
                    || files[0].ContentLength <= 0
                    || files[0].FileName.Length <= 0
                    || files[0].InputStream.Length <= 0)
                {
                    json.state = 2001;
                    json.message = "未获取到文件！";
                    return ToJson(json);
                }
                
                TB_AnchorIdentity identity = new TB_AnchorIdentity()
                {
                    UserId = userId,
                    Remark = ""
                };
                for (int i = 0; i < files.Count; i++)
                {
                    #region OSS存储
                    HttpPostedFileBase file = files[i];
                    string bucket = string.Format("{0}-identity-{1}", OSSConfig.ImageBucketPrefix, DateTime.Now.ToString("yyMM"));
                    string fileName = string.Format("Id{0}{1}{2}", userId, DateTime.Now.ToString("ddHHmmss"), i);
                    string fileKey = "";
                    var checkResult = OSSFileCore.UploadImageStream(file, bucket, fileName, out fileKey);
                    if (checkResult != UploadImageCheckResult.Success)
                    {
                        json.state = (int)checkResult;
                        json.message = checkResult.GetRemark();
                        return ToJson(json);
                    }
                    if (i == 0)
                        identity.IdentityPositive = string.Format("https://{0}.{1}/{2}", bucket, OSSConfig.Host, fileKey);
                    else if (i == 1)
                        identity.IdentityOpposite = string.Format("https://{0}.{1}/{2}", bucket, OSSConfig.Host, fileKey);
                    #endregion

                    #region 本地存储
                    //HttpPostedFileBase file = files[i];
                    //string fileName = string.Format("U{0}{1}{2}", userId, DateTime.Now.ToString("ddHHmmss"), i);
                    //string fileExtension = "";
                    //UploadImageCheckResult checkResult = FileUploadCore.SaveImageFile(file, filePhysicalPath, fileName, out fileExtension);
                    //if (checkResult != UploadImageCheckResult.Success)
                    //{
                    //    json.state = (int)checkResult;
                    //    json.message = checkResult.GetRemark();
                    //    return ToJson(json);
                    //}
                    //if (i == 0)
                    //    identity.IdentityPositive = string.Format("{0}/{1}{2}", fileWebPath, fileName, fileExtension);
                    //else if (i == 1)
                    //    identity.IdentityOpposite = string.Format("{0}/{1}{2}", fileWebPath, fileName, fileExtension); 
                    #endregion
                }

                bool result = service.SaveAnchorIdentity(identity);
                if (!result)
                {
                    json.state = 2002;
                    json.message = "文件上传失败！";
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                return ToJson(json);
            });
        }
        
        // GET: Recommend
        public JsonWebResult GetDevoteRank()
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

                long anchorId = TypeHelper.TryParse(_requestParms.GetValue("anchorid"), 0L); 
                if (anchorId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAnchorService>();
                var list = service.GetDevoteRank(anchorId);
                
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GetCallRatioList()
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

                var dic = Util.GetCallRatioLevel();
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = dic.Select(p => p.Value).ToList();
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult SetCallRatio()
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
                int ratio = TypeHelper.TryParse(_requestParms.GetValue("ratio"), 0);
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                 
                var dic = Util.GetCallRatioLevel();
                if(!dic.ContainsValue(ratio))
                {
                    json.state = -2000;
                    json.message = "呼叫费用不符合设置规定";
                    return ToJson(json);
                }

                var service = Ioc.Get<IAnchorService>();
                BasicTips tips = service.SetCallRate(userId, token,ratio);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GetRandomAnchor()
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
                
                var service = Ioc.Get<IAnchorService>();
                AnchorItem model = service.GetRandomAnchor();
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = model;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult SendBatchMessage()
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
                string message = TypeHelper.TryParse(_requestParms.GetValue("message"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(message))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAnchorService>();
                var tips = service.SendBatchMsg(userId,token,message);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }
    }
}