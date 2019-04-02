using Opcomunity.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services
{
    public class WechatPayConfig
    { 
        public static string APPID
        {
            get { return ConfigHelper.GetValue("WechatPayAppId"); }
        }
        public static string MchID
        {
            get { return ConfigHelper.GetValue("WechatPayMchId"); }
        }
        public static string UnifiedUrl
        {
            get { return ConfigHelper.GetValue("WechatPayUnifiedUrl"); }
        }
        public static string NotifyUrl
        {
            get { return ConfigHelper.GetValue("WechatPayNotifyUrl"); }
        }
        public static string Body
        {
            get { return ConfigHelper.GetValue("WechatPayBody"); }
        }
        public static string TradeType
        {
            get { return ConfigHelper.GetValue("WechatPayTradeType"); }
        }
        public static string ApiKey
        {
            get { return ConfigHelper.GetValue("WechatPayApiKey"); }
        }
        public static string SignType
        {
            get { return ConfigHelper.GetValue("WechatPaySignType"); }
        } 
    }
}
