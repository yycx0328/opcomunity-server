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
    public class VideoController : Controller
    {
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(string userId, string token, string description)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("description", description);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Video/Upload", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(string userId, string token, string category, string pageIndex, string pageSize)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("category", category);
            dic.Add("pageindex", pageIndex);
            dic.Add("pagesize", pageSize);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Video/List", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult Praise()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Praise(string userId, string token, string videoId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("videoid", videoId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Video/Praise", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }


        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string userId, string token, string videoId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("videoid", videoId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Video/Delete", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult Views()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Views(string userId, string token, string videoId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("videoid", videoId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Video/Views", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult MyVideoList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MyVideoList(string userId, string token, string pageIndex, string pageSize)
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
            string url = string.Format("{0}/Video/MyVideoList", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult AnchorVideoList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AnchorVideoList(string userId, string token, string anchorId, string pageIndex, string pageSize)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("anchorid", anchorId);
            dic.Add("pageindex", pageIndex);
            dic.Add("pagesize", pageSize);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Video/AnchorVideoList", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }
    }
}