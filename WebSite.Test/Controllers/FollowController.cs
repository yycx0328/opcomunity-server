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
    public class FollowController : Controller
    { 
        [HttpGet]
        public ActionResult GetFollowUsers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetFollowUsers(string userId, string token, string pageIndex, string pageSize)
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
            string url = string.Format("{0}/Follow/GetFollowUsers", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }
        [HttpGet]
        public ActionResult GetFollowMineUsers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetFollowMineUsers(string userId, string token, string pageIndex, string pageSize)
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
            string url = string.Format("{0}/Follow/GetFollowMineUsers", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult FollowUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FollowUser(string userId, string token, string followedUserId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("followeduserid", followedUserId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Follow/FollowUser", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }
         
        [HttpGet]
        public ActionResult CancelFollow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CancelFollow(string userId, string token, string followedUserId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("followeduserid", followedUserId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Follow/CancelFollow", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }
    }
}