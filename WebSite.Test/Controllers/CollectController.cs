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
    public class CollectController : Controller
    {
        [HttpGet]
        public ActionResult GetCollectTopics()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCollectTopics(string userId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Collect/GetCollectTopics", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult CollectTopic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CollectTopic(string userId, string topicId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("topicid", topicId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Collect/CollectTopic", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult CancelCollect()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CancelCollect(string userId, string topicId)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("topicid", topicId);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Collect/CancelCollect", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }
    }
}