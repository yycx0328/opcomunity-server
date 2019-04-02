using Infrastructure;
using log4net;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utility.Common;
using WebSite.Common;

namespace WebSite.Controllers
{
    public class AccountController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        //private static readonly string StaticFileBasePhysicalPath = ConfigurationManager.AppSettings["StaticFileBasePhysicalPath"];
        //private static readonly string StaticFileWebRoot = ConfigurationManager.AppSettings["StaticFileWebRoot"];

        // GET: Recommend
        public JsonWebResult GeUserDetailInfo()
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
                string version = TypeHelper.TryParse(_requestParms.GetValue("version"), "");
                if (string.IsNullOrEmpty(version))
                {
                    json.state = (int)CheckTips.VersionTooOld;
                    json.message = CheckTips.VersionTooOld.GetRemark();
                    return ToJson(json);
                }

                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>(); 
                CheckTips tips = CheckTips.Init;
                var detailInfo = service.GetUserDetailInfo(userId,token,out tips); 

                json.state = (int)tips;
                json.message = tips.GetRemark();
                json.data = detailInfo;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GeOthersDetailInfo()
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

                long userId = TypeHelper.TryParse(_requestParms.GetValue("othersid"), 0L);

                // 页码从1开始
                if (userId <= 0 )
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                CheckTips tips = CheckTips.Init;
                var detailInfo = service.GetOthersDetailInfo(userId, out tips);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                json.data = detailInfo;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GetUserInfoByAccId()
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

                string accId = TypeHelper.TryParse(_requestParms.GetValue("accid"), "");

                // 页码从1开始
                if (string.IsNullOrEmpty(accId))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>(); 
                var detailInfo = service.GetUserInfoByAccId(accId);
                if(detailInfo == null)
                {
                    json.state = 2000;
                    json.message = "用户信息错误";
                    return ToJson(json);
                }
                
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = detailInfo;
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GeCoinCount()
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

                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                BasicTips tips = BasicTips.Init;
                var coinModel = service.GetCoinCount(userId, token, out tips);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                json.data = new {
                    CurrentCoin = coinModel.CurrentCoin,
                    CurrentIncome = coinModel.CurrentIncome
                };
                return ToJson(json);
            });
        }

        public JsonWebResult ModifyNickName()
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

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                string nickName = TypeHelper.TryParse(_requestParms.GetValue("nickname"), ""); 
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(nickName))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                var tips = service.ModifyNickName(userId, token, nickName);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        public JsonWebResult ModifyDescription()
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

                var service = Ioc.Get<IAccountService>();
                var tips = service.ModifyDescription(userId, token, description);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }
         
        // GET: Recommend
        public JsonWebResult ModifyAvatar()
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

                var service = Ioc.Get<IAccountService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                #region 保存用户头像文件
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

                #region OSS存储
                HttpPostedFileBase file = files[0];
                string bucket = string.Format("{0}-avatar-{1}",OSSConfig.ImageBucketPrefix, DateTime.Now.ToString("yyMM"));
                Log4NetHelper.Info(log, bucket);
                string fileName = string.Format("Id{0}{1}", userId, DateTime.Now.ToString("ddHHmmss"));
                string fileKey = "";
                var checkResult = OSSFileCore.UploadImageStream(file, bucket, fileName, out fileKey);
                if (checkResult != UploadImageCheckResult.Success)
                {
                    json.state = (int)checkResult;
                    json.message = checkResult.GetRemark();
                    return ToJson(json);
                }
                string avatarUrl = string.Format("http://{0}.{1}/{2}", bucket, OSSConfig.Host, fileKey);
                string thumbAvatarUrl = string.Format("{0}{1}", avatarUrl, "?x-oss-process=image/resize,w_240");
                #endregion

                //#region 本地存储
                //string StaticFileBasePhysicalPath = ConfigurationManager.AppSettings["StaticFileBasePhysicalPath"];
                //string StaticFileWebRoot = ConfigurationManager.AppSettings["StaticFileWebRoot"];
                //string subDir = DateTime.Now.ToString("yyMM");
                //string filePhysicalPath = string.Format("{0}/images/avatar/{1}", StaticFileBasePhysicalPath, subDir);
                //string fileWebPath = string.Format("{0}/images/avatar/{1}", StaticFileWebRoot, subDir);

                //HttpPostedFileBase file = files[0];
                //string fileName = string.Format("U{0}{1}", userId, DateTime.Now.ToString("ddHHmmss"));
                //string fileExtension = "";
                //UploadImageCheckResult checkResult = FileUploadCore.SaveImageFile(file, filePhysicalPath, fileName, out fileExtension);
                //if (checkResult != UploadImageCheckResult.Success)
                //{
                //    json.state = (int)checkResult;
                //    json.message = checkResult.GetRemark();
                //    return ToJson(json);
                //}
                //string avatarUrl = string.Format("{0}/{1}{2}", fileWebPath, fileName, fileExtension);
                //string thumbAvatarUrl = string.Format("{0}/{1}S{2}", fileWebPath, fileName, fileExtension); 
                //#endregion

                #endregion

                #region 调用接口更新头像
                var tips = service.ModifyAvatar(userId, avatarUrl, thumbAvatarUrl);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
                #endregion
            });
        }

        [HttpPost]
        public JsonWebResult UploadLifeShow()
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

                HttpFileCollectionBase files = (HttpFileCollectionBase)Request.Files;
                if (files == null || files.Count <= 0
                    || files[0].ContentLength <= 0
                    || files[0].FileName.Length <= 0
                    || files[0].InputStream.Length <= 0)
                {
                    json.state = 2001;
                    json.message = "未获取到文件";
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                if(!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                #region OSS存储
                string bucket = string.Format("{0}-photo-{1}", OSSConfig.ImageBucketPrefix, DateTime.Now.ToString("yyMM"));
                List<TB_UserPhoto> list = new List<TB_UserPhoto>();
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fileName = string.Format("Id{0}{1}{2}", userId, DateTime.Now.ToString("ddHHmmss"), i);
                    string fileKey = "";
                    var checkResult = OSSFileCore.UploadImageStream(file, bucket, fileName, out fileKey);
                    if (checkResult != UploadImageCheckResult.Success)
                    {
                        json.state = (int)checkResult;
                        json.message = checkResult.GetRemark();
                        return ToJson(json);
                    }
                    
                    string imgUrl = string.Format("http://{0}.{1}/{2}", bucket, OSSConfig.Host, fileKey);
                    list.Add(new TB_UserPhoto
                    {
                        UserId = userId,
                        SortId = i,
                        ImageWebPath = imgUrl,
                        ThumbnailPath = string.Format("{0}{1}", imgUrl, "?x-oss-process=image/resize,w_240"),
                        CreateTime = DateTime.Now
                    });
                }
                #endregion

                #region 本地存储
                //string subDir = DateTime.Now.ToString("yyMM");
                //string filePhysicalPath = string.Format("{0}/images/uphoto/{1}", StaticFileBasePhysicalPath, subDir);
                //string fileWebPath = string.Format("{0}/images/uphoto/{1}", StaticFileWebRoot, subDir);

                //List<TB_UserPhoto> list = new List<TB_UserPhoto>();
                //for (int i = 0; i < files.Count; i++)
                //{
                //    HttpPostedFileBase file = files[i];
                //    string fileName = string.Format("Id{0}{1}{2}", userId, DateTime.Now.ToString("ddHHmmss"), i);
                //    string fileExtension = "";
                //    UploadImageCheckResult checkResult = FileUploadCore.SaveImageFile(file, filePhysicalPath, fileName, out fileExtension);
                //    if (checkResult != UploadImageCheckResult.Success)
                //    {
                //        json.state = (int)checkResult;
                //        json.message = checkResult.GetRemark();
                //        return ToJson(json);
                //    }

                //    list.Add(new TB_UserPhoto
                //    {
                //        UserId = userId,
                //        SortId = i,
                //        ImageWebPath = string.Format("{0}/{1}{2}", fileWebPath, fileName, fileExtension),
                //        ThumbnailPath = string.Format("{0}/{1}S{2}", fileWebPath, fileName, fileExtension),
                //        CreateTime = DateTime.Now
                //    });
                //} 
                #endregion

                if (!service.SaveLifeShowImages(list))
                {
                    json.state = 3000;
                    json.message = "图片上传失败！";
                    return ToJson(json);
                }

                var photos = service.GetLifeShowImages(userId);
                if(photos==null)
                {
                    json.state = 3001;
                    json.message = "未获取到照片集！";
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = from p in photos select new
                {
                    p.Id,
                    p.ImageWebPath,
                    p.ThumbnailPath
                };
                return ToJson(json);
            });
        }

        [HttpPost]
        public JsonWebResult DeleteLifeShow()
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
                long pictureId = TypeHelper.TryParse(_requestParms.GetValue("pictureid"), 0L);
                if (userId <= 0 || string.IsNullOrEmpty(token) || pictureId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                DeleteLifeShowTips tips = service.DeleteLifeShowImage(userId, token, pictureId);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        /// <summary>
        /// 记录时长为0的视频通话记录
        /// </summary>
        /// <returns></returns>
        public JsonWebResult LiveCallRecord()
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
                long liveId = TypeHelper.TryParse(_requestParms.GetValue("liveid"), 0L);
                int sender = TypeHelper.TryParse(_requestParms.GetValue("sender"), 0);
                DateTime startTime = TypeHelper.TryParse(_requestParms.GetValue("start"), default(DateTime));
                DateTime endTime = TypeHelper.TryParse(_requestParms.GetValue("end"), default(DateTime));
                if (userId <= 0 || anchorId <= 0 || sender < 0 || liveId<=0
                    || startTime == default(DateTime) || endTime == default(DateTime))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if (userId == anchorId)
                {
                    json.state = (int)LiveCallTips.CannotCallYourselfErr;
                    json.message = LiveCallTips.CannotCallYourselfErr.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                var tips = service.LiveCallRecord(userId, anchorId, token, liveId, sender,  startTime, endTime);

                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult Cash()
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
                int money = TypeHelper.TryParse(_requestParms.GetValue("money"), 0);
                string cashAccount = TypeHelper.TryParse(_requestParms.GetValue("cashaccount"), "");
                string cashName = TypeHelper.TryParse(_requestParms.GetValue("cashname"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || money <= 0
                    || string.IsNullOrEmpty(cashAccount) || string.IsNullOrEmpty(cashName))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                long currentIncome = 0;
                CashOutTips tips = service.CashOut(userId, token, cashAccount, cashName, money, out currentIncome);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                if (tips == CashOutTips.Success)
                    json.data = new { CurrentIncome = currentIncome };
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult SendGift()
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
                int giftId = TypeHelper.TryParse(_requestParms.GetValue("giftid"), 0);

                // 页码从1开始
                if (userId<=0 || string.IsNullOrEmpty(token) || anchorId <= 0 || giftId <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if (userId == anchorId)
                {
                    json.state = (int)SendGiftTips.CannotSendGiftForYourselfErr;
                    json.message = SendGiftTips.CannotSendGiftForYourselfErr.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                long currentCoinCount = 0;
                var tips = service.SendGiftTransaction(userId, token, anchorId,giftId, out currentCoinCount);
                if (tips != SendGiftTips.Success)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    json.data = new { CurrentCoin = currentCoinCount };
                    return ToJson(json);
                }
                // 推送发送礼物消息给主播
                service.JPushMessageAfterSendGift(userId, anchorId, giftId);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { CurrentCoin = currentCoinCount };
                return ToJson(json);
            });
        }
         
        // GET: Recommend
        public JsonWebResult TrasactionRecord()
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
                if (userId <= 0 || string.IsNullOrEmpty(token) || pageIndex<=0 || pageSize<=0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<ICoinService>();
                if(!service.IsLoginUser(userId,token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }
                var list = service.GetCoinTransactionRecord(userId, pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = "成功";
                json.data = list;
                return ToJson(json);
            });
        }
        
        // GET: Recommend
        public JsonWebResult IncomeRecord()
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

                var service = Ioc.Get<ICoinService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }
                var list = service.GetCoinIncomeRecord(userId, pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = "成功";
                json.data = list;
                return ToJson(json);
            });
        }

        public JsonWebResult SetNeteaseLoginStatus()
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

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                int status = TypeHelper.TryParse(_requestParms.GetValue("status"), -1);
                if (userId <= 0 || string.IsNullOrEmpty(token) || status < 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                SetNeteaseLoginOrChatStatusTips tips = service.SetNeteaseLoginStatus(userId, token, status);
                if (tips!=SetNeteaseLoginOrChatStatusTips.Success)
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

        public JsonWebResult SetNeteaseChatStatus()
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

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                int status = TypeHelper.TryParse(_requestParms.GetValue("status"), -1);
                if (userId <= 0 || string.IsNullOrEmpty(token) || status < 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                SetNeteaseLoginOrChatStatusTips tips = service.SetNeteaseChatStatus(userId, token, status);
                if (tips != SetNeteaseLoginOrChatStatusTips.Success)
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

        public JsonWebResult LiveCallHistory()
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
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                LiveCallHistoryTips tips = LiveCallHistoryTips.Init;
                var list = service.GetLiveCallHistory(userId, token, pageIndex,pageSize,out tips);
                if (tips != LiveCallHistoryTips.Success)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                //if(list!=null)
                //{
                //    foreach(var item in list) 
                //        if (item.IsUnConnected) item.CallTime *= 1000; 
                //}
                json.data = list;
                return ToJson(json);
            });
        }

        public JsonWebResult InviteRewardList()
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
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                BasicTips tips = BasicTips.Init;
                var list = service.GetInviteRewardList(userId, token, pageIndex, pageSize, out tips);
                if (tips != BasicTips.Success)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        public JsonWebResult InviteStatistics()
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

                var service = Ioc.Get<IAccountService>();
                BasicTips tips = BasicTips.Init;
                var model = service.GetInviteStatistics(userId, token, out tips);
                if (tips != BasicTips.Success)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = model;
                return ToJson(json);
            });
        }

        public JsonWebResult IncomeStatistics()
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

                var service = Ioc.Get<IAccountService>();
                BasicTips tips = BasicTips.Init;
                var model = service.GetIncomeStatistics(userId, token, out tips);
                if (tips != BasicTips.Success)
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = model;
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult GetUnReadMeassageCount()
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
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                var count = service.GetUnReadMessageCount(userId, token);
                if (count == -1)
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = count;
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult SetMessageReadStatus()
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
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                var tips = service.SetMessageReadStatus(userId, token);
                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }


        // GET: Recommend
        public JsonWebResult AddCoin()
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
                int coinCount = TypeHelper.TryParse(_requestParms.GetValue("count"), 0);

                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token) || coinCount <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                
                var service = Ioc.Get<IAccountService>();
                var tips = service.AddCoin(userId, token, coinCount);

                json.state = (int)tips;
                json.message = tips.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult LiveCallRecordPerMinute()
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
                long channelId = TypeHelper.TryParse(_requestParms.GetValue("messageid"), 0L);
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                var tips = NeteaseCallNotifyTips.Init;
                var count = service.LiveCallRecordPerMinute(userId, token,anchorId, channelId,60,out tips);
                if (count <=0 )
                {
                    json.state = (int)tips;
                    json.message = tips.GetRemark();
                    json.data = new { CurrentCoin = count };
                    return ToJson(json);
                }

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { CurrentCoin = count };
                return ToJson(json);
            });
        }

        public JsonWebResult Visit()
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
                int channel = TypeHelper.TryParse(_requestParms.GetValue("channel"), 0);
                string version = TypeHelper.TryParse(_requestParms.GetValue("version"), "");
                string os = TypeHelper.TryParse(_requestParms.GetValue("os"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || channel<=0 || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(os))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var model = new TB_AppVisitLog()
                {
                    UserId = userId,
                    Channel = channel,
                    Version = version,
                    OS = os,
                    CreateTime = DateTime.Now
                };

                var service = Ioc.Get<IAccountService>();
                service.SaveVisitLog(model);
                service.SaveWaitingSendMessage(userId);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                return ToJson(json);
            });
        }

        // GET: Recommend
        public JsonWebResult GetUserList()
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
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }
                UserCategoryConfig config = UserCategoryConfig.Normal;
                Enum.TryParse(category.ToString(), out config);
                var service = Ioc.Get<IAccountService>();
                List<NormalUserItem> list = service.GetUserList(config, pageIndex, pageSize);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = list;
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult GetTotalRemainderTicketCount()
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
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                string totalRemaindCount = "0";
                var tips = service.GetTotalRemainderTicketCount(userId, token, out totalRemaindCount);

                json.state = (int)tips;
                json.message = tips.GetRemark();
                json.data = new
                {
                    TotalRemaindCount = totalRemaindCount
                };
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult GetChatTicketCount()
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
                string accId = TypeHelper.TryParse(_requestParms.GetValue("accid"), "");
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(accId))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                int remaindCount = 0;
                bool isLimit = true;
                var tips = service.GetChatTicketCount(userId, token,accId, out remaindCount, out isLimit);

                json.state = (int)tips;
                json.message = tips.GetRemark();
                json.data = new
                {
                    IsLimit = isLimit,
                    RemindCount = remaindCount
                };
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult RecordChatTicketCount()
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
                int count = TypeHelper.TryParse(_requestParms.GetValue("count"), 0);
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                bool isLimit = true;
                int remaindCount = 0;
                var tips = service.RecordChatTicketCount(userId, token, anchorId,count,out isLimit, out remaindCount);

                json.state = (int)tips;
                json.message = tips.GetRemark();
                json.data = new
                {
                    IsLimit = isLimit,
                    RemindCount = remaindCount
                };
                return ToJson(json);
            });
        }

        // GET: Default
        public JsonWebResult GetRemindMessageCount()
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
                // 页码从1开始
                if (userId <= 0 || string.IsNullOrEmpty(token))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IAccountService>();
                if(!service.IsLoginUser(userId,token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                bool isLimit = true;
                var count = service.GetRemindMessageCount(userId, token,out isLimit);

                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new
                {
                    IsLimit = isLimit,
                    RemindCount = count
                };
                return ToJson(json);
            });
        }
    }
}