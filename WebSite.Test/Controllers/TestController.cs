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
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult DataInitialize()
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>(); 
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Default/Index", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View(); 
        }

        // GET: Test
        public ActionResult RunNeteaseAccount()
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Invite/RunNeteaseAccount", "http://api.opcomunity.com");
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        public ActionResult Signature(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            string _sign_values = "";
            string _ret_values = "";
            foreach (var item in dic)
            {
                _ret_values += string.Format("{0}={1}&", item.Key, item.Value);
                _sign_values += string.Format("{0}={1}", item.Key, item.Value);
            }
            string sign = WebUtils.MD5(_sign_values + securityKey, "UTF-8").ToLower();
            _ret_values += string.Format("sign={0}", sign); 
            ViewData["Result"] = _ret_values;
            return View();
        }
    }
}