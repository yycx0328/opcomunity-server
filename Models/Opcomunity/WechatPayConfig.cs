using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Opcomunity
{
    public class WechatPayConfig
    {
        public static string APPID = "xxx";
        public static string MchID = "xxx"; 
        public static string UnifiedUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        public static string NotifyUrl = ConfigurationManager.AppSettings["WechatCallbackUrl"];
        public static string Body = "圈币";
        public static string TradeType = "APP";
        public static string ApiKey = "xxx"; 
        public static string SignType = "MD5";
    }
}
