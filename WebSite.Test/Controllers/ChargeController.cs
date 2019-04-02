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
    public class ChargeController : Controller
    {
        [HttpGet]
        public ActionResult AlipayTakeOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlipayTakeOrder(string userId,string token, string amount)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("amount", amount); 
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Charge/AlipayTakeOrder", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult WechatPayTakeOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WechatPayTakeOrder(string userId, string token, string amount)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("amount", amount);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Charge/WechatPayTakeOrder", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult VipAlipayTakeOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VipAlipayTakeOrder(string userId, string token, string vipType)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("viptype", vipType);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Charge/VipAlipayTakeOrder", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult VipWechatPayTakeOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VipWechatPayTakeOrder(string userId, string token, string vipType)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("viptype", vipType);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Charge/VipWechatPayTakeOrder", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GetVipChargeMoneyConfig()
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Charge/GetVipChargeMoneyConfig", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult TicketAlipayTakeOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TicketAlipayTakeOrder(string userId, string token, string ticketType)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("tickettype", ticketType);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Charge/TicketAlipayTakeOrder", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult TicketWechatPayTakeOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TicketWechatPayTakeOrder(string userId, string token, string ticketType)
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userid", userId);
            dic.Add("token", token);
            dic.Add("tickettype", ticketType);
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Charge/TicketWechatPayTakeOrder", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult GetTicketConfig()
        {
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("timespan", timeSpan);
            NameValueCollection data = Util.GetPostDataCollection(dic, securityKey);
            string url = string.Format("{0}/Charge/GetTicketConfig", ConfigurationManager.AppSettings["ApiBaseUrl"]);
            string result = WebUtils.PostDataToUrl(url, Encoding.UTF8, data);

            ViewData["Result"] = result;
            return View();
        }
    }
}