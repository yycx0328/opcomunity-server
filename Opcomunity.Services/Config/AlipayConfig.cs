using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Opcomunity.Services.Helpers;

namespace Opcomunity.Services
{
    public class AlipayConfig
    { 
        public static string APPID
        {
            get { return ConfigHelper.GetValue("AlipayAppId"); }
        }
        public static string APP_PRIVATE_KEY
        {
            get { return ConfigHelper.GetValue("AlipayPrivateKey"); }
        }
        public static string ALIPAY_PUBLIC_KEY
        {
            get { return ConfigHelper.GetValue("AlipayPublicKey"); }
        }
        public static string CHARSET
        {
            get { return ConfigHelper.GetValue("AlipayCharset"); }
        }
        public static string ALIPAY_GATEWAY
        {
            get { return ConfigHelper.GetValue("AlipayGateway"); }
        }
        public static string NOTIFY_URL
        {
            get { return ConfigHelper.GetValue("AlipayNotifyUrl"); }
        }
        public static string VIP_NOTIFY_URL
        {
            get { return ConfigHelper.GetValue("AlipayVipNotifyUrl"); }
        }
        public static string TICKET_NOTIFY_URL
        {
            get { return ConfigHelper.GetValue("AlipayTicketNotifyUrl"); }
        }
        public static string SELLER_ID
        {
            get { return ConfigHelper.GetValue("AlipaySellerId"); }
        }
    }
}