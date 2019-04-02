using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class WechatPayClientParamters
    {
        public string orderid { get; set; }     // 订单号
        public string appid { get; set; }       // 开放平台账户的唯一标识
        public string noncestr { get; set; }    // 随机字符串
        public string package { get; set; }     // package
        public string partnerid { get; set; }   // 商户ID
        public string prepayid { get; set; }    // 预支付ID
        public string sign { get; set; }        // 签名
        public string timestamp { get; set; }   // 时间戳
    }
}