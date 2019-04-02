using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Test.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult GeUserDetailInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeUserDetailInfo(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GeUserDetailInfo", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GeOthersDetailInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeOthersDetailInfo(string othersId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("othersid", othersId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GeOthersDetailInfo", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GeCoinCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeCoinCount(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GeCoinCount", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GetUserInfoByAccId()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUserInfoByAccId(string accId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("accid", accId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GetUserInfoByAccId", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult ModifyNickName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyNickName(string userId, string token, string nickName)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("nickname", nickName);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/ModifyNickName", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult ModifyDescription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyDescription(string userId, string token, string description)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("description", description);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/ModifyDescription", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult ModifyAvatar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyAvatar(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token); 
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/ModifyAvatar", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult UploadLifeShow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadLifeShow(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/UploadLifeShow", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }
        
        [HttpGet]
        public ActionResult LiveCallRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LiveCallRecord(string userId,string anchorId, string sender, string token, string liveId,  string start, string end)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("anchorid", anchorId);
            dic.Add("liveid", liveId);
            dic.Add("sender", sender);
            dic.Add("start", start);
            dic.Add("end", end);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/LiveCallRecord", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult SendGift()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendGift(string userId, string token, string anchorId, string giftId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("anchorid", anchorId);
            dic.Add("giftid", giftId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/SendGift", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult TrasactionRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TrasactionRecord(string userId, string token, string pageIndex, string pageSize)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("pageindex", pageIndex);
            dic.Add("pagesize", pageSize);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/TrasactionRecord", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult IncomeRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IncomeRecord(string userId, string token, string pageIndex, string pageSize)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("pageindex", pageIndex);
            dic.Add("pagesize", pageSize);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/IncomeRecord", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult SetNeteaseLoginStatus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetNeteaseLoginStatus(string userId, string token, string status)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("status", status);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/SetNeteaseLoginStatus", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult SetNeteaseChatStatus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetNeteaseChatStatus(string userId, string token, string status)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("status", status);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/SetNeteaseChatStatus", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult LiveCallHistory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LiveCallHistory(string userId, string token, string pageIndex, string pageSize)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("pageindex", pageIndex);
            dic.Add("pagesize", pageSize);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/LiveCallHistory", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult DeleteLifeShow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteLifeShow(string userId, string token, string pictureId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("pictureid", pictureId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/DeleteLifeShow", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult InviteRewardList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InviteRewardList(string userId, string token, string pageIndex, string pageSize)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("pageindex", pageIndex);
            dic.Add("pagesize", pageSize);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/InviteRewardList", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult InviteStatistics()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InviteStatistics(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/InviteStatistics", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult IncomeStatistics()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IncomeStatistics(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/IncomeStatistics", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GetUnReadMeassageCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUnReadMeassageCount(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GetUnReadMeassageCount", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult SetMessageReadStatus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetMessageReadStatus(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/SetMessageReadStatus", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult AddCoin()
        {
            return View();
        }
         
        [HttpPost]
        public ActionResult AddCoin(string userId, string token,string count)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("count", count);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/AddCoin", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult Cash()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cash(string userId, string token, string money, string cashAccount, string cashName)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("cashaccount", cashAccount);
            dic.Add("cashname", cashName);
            dic.Add("money", money);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/Cash", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult LiveCallRecordPerMinute()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LiveCallRecordPerMinute(string userId, string token, string anchorId, string messageId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("anchorid", anchorId);
            dic.Add("messageid", messageId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/LiveCallRecordPerMinute", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult Visit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Visit(string userId, string token, string version, string os)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("version", version);
            dic.Add("os", os);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/Visit", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GetUserList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUserList(string category, string pageIndex, string pageSize)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("category", category);
            dic.Add("pageindex", pageIndex);
            dic.Add("pagesize", pageSize);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GetUserList", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult CheckSendMessagePermission()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckSendMessagePermission(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/CheckSendMessagePermission", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GetRemindMessageCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetRemindMessageCount(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GetRemindMessageCount", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GetTotalRemainderTicketCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTotalRemainderTicketCount(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GetTotalRemainderTicketCount", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GetChatTicketCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetChatTicketCount(string userId, string token,string accId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("accid", accId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/GetChatTicketCount", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult RecordChatTicketCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecordChatTicketCount(string userId, string token, string anchorId, string count)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("anchorid", anchorId);
            dic.Add("count", count);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Account/RecordChatTicketCount", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }
    }
}