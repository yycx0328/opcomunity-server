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
        public static string APPID = "wxdb246e06e98f87e4";
        public static string MchID = "1497669662"; 
        public static string UnifiedUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        public static string NotifyUrl = ConfigurationManager.AppSettings["WechatCallbackUrl"];
        public static string Body = "圈币";
        public static string TradeType = "APP";
        public static string ApiKey = "87c6fc4251974716b0b94f1fdc349d8b"; 
        public static string SignType = "MD5";
    }
}
