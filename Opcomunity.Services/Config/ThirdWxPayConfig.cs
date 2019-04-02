using Opcomunity.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Config
{
    public class ThirdWxPayConfig
    {
        public static string AppId
        {
            get { return ConfigHelper.GetValue("6K29WxAppId"); }
        }
        public static string AppKey
        {
            get { return ConfigHelper.GetValue("6K29WxAppKey"); }
        }
        public static string ItemName
        {
            get { return ConfigHelper.GetValue("6K29WxItemName"); }
        }
        public static string NotifyUrl
        {
            get { return ConfigHelper.GetValue("6K29WxNotifyUrl"); }
        }
        public static string VipNotifyUrl
        {
            get { return ConfigHelper.GetValue("6K29VipWxNotifyUrl"); }
        }
        public static string TicketNotifyUrl
        {
            get { return ConfigHelper.GetValue("6K29TicketWxNotifyUrl"); }
        }
        public static string OrderDesc
        {
            get { return ConfigHelper.GetValue("6K29WxOrderDesc"); }
        }
        public static string PayType
        {
            get { return ConfigHelper.GetValue("6K29WxPayType"); }
        }
        public static string PayWay
        {
            get { return ConfigHelper.GetValue("6K29WxPayWay"); }
        }
        public static string PreorderApi
        {
            get { return ConfigHelper.GetValue("6K29WxPreorderApi"); }
        }
    }
}
