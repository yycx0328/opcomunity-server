using Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Web.Mvc;

namespace WebSite.Test.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult GetPhoneVerifyCode()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPhoneVerifyCode(string phoneNo)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("phoneno", phoneNo);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Login/GetPhoneVerifyCode",ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult PhoneRegist()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PhoneRegist(string phoneNo, string code,string messageId, string digest)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("phoneno", phoneNo);
            dic.Add("code",code);
            dic.Add("messageid", messageId);
            dic.Add("digest", digest);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Login/PhoneRegist", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }
        
        [HttpGet]
        public ActionResult PhoneLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PhoneLogin(string phoneNo, string code, string digest)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("phoneno", phoneNo);
            dic.Add("digest", digest);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Login/PhoneLogin", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult PhoneRestPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PhoneRestPassword(string phoneNo, string code, string messageId, string digest)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("phoneno", phoneNo);
            dic.Add("code", code);
            dic.Add("messageid", messageId);
            dic.Add("digest", digest);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Login/PhoneRestPassword", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult LoginToNetease()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginToNetease(string userId, string token)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Login/LoginToNetease", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult EncryptPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EncryptPassword(string password)
        {
            string desKey = System.Configuration.ConfigurationManager.AppSettings["3DESKEY"];
            string desIV = System.Configuration.ConfigurationManager.AppSettings["3DESIV"];

            // 将客户端传递的密码进行3DE加密
            password = SecurityHelper.TripleDESEncrypt(password, desKey, desIV);
            ViewData["Result"] = password;
            return View();
        }

        [HttpGet]
        public ActionResult VisitorLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VisitorLogin(string uuid)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("uuid", uuid);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Login/VisitorLogin", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);
            ViewData["Result"] = result;
            return View();
        }

    }
}