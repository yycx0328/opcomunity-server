using log4net;
using Newtonsoft.Json.Linq;
using Opcomunity.Services;
using Opcomunity.Services.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebSite.Models
{
    public class ThirdWxPay
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        public WechatPayClientParamters GetPerOrderString(string orderId, decimal amount, int source, string type)
        {
            #region 拼接MD5加密串
            String[] param = new String[6];
            param[0] = "appid=" + ThirdWxPayConfig.AppId;
            param[1] = "amount=" + amount;
            param[2] = "itemname=" + ThirdWxPayConfig.ItemName;
            param[3] = "ordersn=" + orderId;
            param[4] = "orderdesc=" + ThirdWxPayConfig.OrderDesc;
            if (type == OrderTypeConfig.None.ToString())
                param[5] = "notifyurl=" + ThirdWxPayConfig.NotifyUrl;
            else if (type == OrderTypeConfig.Limit.ToString() || type == OrderTypeConfig.UnLimit.ToString())
                param[5] = "notifyurl=" + ThirdWxPayConfig.TicketNotifyUrl;
            else
                param[5] = "notifyurl=" + ThirdWxPayConfig.VipNotifyUrl;

            Array.Sort(param);

            String str = "";
            bool flag = false;
            for (int i = 0; i < param.Length; i++)
            {
                Console.WriteLine(param[i] + "  ");

                if (!"".Equals(param[i]))
                {
                    if (!flag)
                    {
                        str = param[i].Split('=')[1];
                        flag = true;
                    }
                    else
                    {
                        str += "|" + param[i].Split('=')[1];
                    }
                }

            }

            String signstr = str + "|" + ThirdWxPayConfig.AppKey;
            String sign = WebUtils.MD5(signstr, "UTF-8").ToLower();
            Log4NetHelper.Info(log, "拼接签名字符串：" + signstr);
            Log4NetHelper.Info(log, "计算得到的MD5值" + sign);
            #endregion

            #region 发送POST请求
            NameValueCollection nv = new NameValueCollection();
            nv.Add("appid", ThirdWxPayConfig.AppId);
            nv.Add("amount", amount.ToString());
            nv.Add("itemname", ThirdWxPayConfig.ItemName);
            nv.Add("ordersn", orderId);
            nv.Add("orderdesc", ThirdWxPayConfig.OrderDesc);

            if (type == OrderTypeConfig.None.ToString())
                nv.Add("notifyurl", ThirdWxPayConfig.NotifyUrl);
            else if (type == OrderTypeConfig.Limit.ToString() || type == OrderTypeConfig.UnLimit.ToString())
                nv.Add("notifyurl", ThirdWxPayConfig.TicketNotifyUrl);
            else
                nv.Add("notifyurl", ThirdWxPayConfig.VipNotifyUrl);
            
            nv.Add("sign", sign);
            nv.Add("source", source.ToString());
            nv.Add("returnurl", "");
            nv.Add("payway", ThirdWxPayConfig.PayWay);
            nv.Add("ext", "");
            nv.Add("paytype", ThirdWxPayConfig.PayType);
            string res = WebUtils.PostDataToUrl(ThirdWxPayConfig.PreorderApi, Encoding.UTF8, nv);
            Log4NetHelper.Info(log, "预订单字符串：" + res); 
            #endregion
            if(!string.IsNullOrEmpty(res))
            {
                var jObj = JObject.Parse(res);
                if(jObj["status"].ToString() == "1")
                { 
                    WechatPayClientParamters _client_param = new WechatPayClientParamters()
                    {
                        orderid = orderId,
                        appid = jObj["data"]["appid"].ToString(),
                        noncestr = jObj["data"]["noncestr"].ToString(),
                        package = jObj["data"]["package"].ToString(),
                        partnerid = jObj["data"]["partnerid"].ToString(),
                        prepayid = jObj["data"]["prepayid"].ToString(),
                        timestamp = jObj["data"]["timestamp"].ToString(),
                        sign = jObj["data"]["sign"].ToString(),
                    };
                    return _client_param;
                } 
            }
            return null;
        }

        static string GetContent(string url)
        {
            string content;
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpRequest.Referer = url;
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)";
            httpRequest.Accept = "text/html, application/xhtml+xml, */*";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.Method = "GET";

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                using (Stream responsestream = httpResponse.GetResponseStream())
                {

                    using (StreamReader sr = new StreamReader(responsestream, System.Text.Encoding.UTF8))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogHelper.Instance.WriteExceptionLog(e);
                return "";
            }

            return content;
        }
         
        public static String getMD5(String str)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}