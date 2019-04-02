using log4net;
using Opcomunity.Services;
using Opcomunity.Services.Helpers;
using System;
using System.Reflection;
using System.Web;
using System.Xml;

namespace WebSite.Models
{
    public class WechatPay
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        public WechatPayClientParamters GetClientParamters(HttpContextBase httpContext, string orderId, decimal amount)
        {
            #region 基本参数===========================
            string _time_stamp = TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now).ToString(); //时间戳 
            Random random = new Random();
            string _nonce_str = WebUtils.GetMD5(random.Next(1000).ToString(), "GBK");   //随机字符串  
            #endregion

            #region 生成Sign签名及拼接要发送的xml========
            var _xmlReqHandler = new WechatRequest(httpContext);    //创建支付应答对象
            _xmlReqHandler.init();   //初始化
                                     //设置package订单参数  具体参数列表请参考官方pdf文档，请勿随意设置
            _xmlReqHandler.setParameter("appid", WechatPayConfig.APPID);
            _xmlReqHandler.setParameter("mch_id", WechatPayConfig.MchID);
            _xmlReqHandler.setParameter("nonce_str", _nonce_str);
            _xmlReqHandler.setParameter("sign_type", WechatPayConfig.SignType);
            _xmlReqHandler.setParameter("body", WechatPayConfig.Body); //商品信息 127字符
            _xmlReqHandler.setParameter("out_trade_no", orderId); //商家订单号
            _xmlReqHandler.setParameter("total_fee", (amount * 100).ToString()); //商品金额,以分为单位(money * 100).ToString()
            _xmlReqHandler.setParameter("spbill_create_ip", httpContext.Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
            _xmlReqHandler.setParameter("notify_url", WechatPayConfig.NotifyUrl);
            _xmlReqHandler.setParameter("trade_type", WechatPayConfig.TradeType);
            string _sign = _xmlReqHandler.CreateMd5Sign("key", WechatPayConfig.ApiKey);    // 生成sign
            _xmlReqHandler.setParameter("sign", _sign); 

            string data = _xmlReqHandler.parseXML();    // 拼接成xml格式数据提交
            Log4NetHelper.Info(log,"wechat pay data:" + data);
            string prepayXml = WebUtils.Send(data, WechatPayConfig.UnifiedUrl);    // 发送xml参数到微信服务器 

            #endregion
            //获取预支付ID
            var xdoc = new XmlDocument();
            xdoc.LoadXml(prepayXml);
            XmlNode xn = xdoc.SelectSingleNode("xml");

            /* 成功参数结果
             *  <xml>
             *    <return_code><![CDATA[SUCCESS]]></return_code>  
             *    <return_msg><![CDATA[OK]]></return_msg>  
             *    <appid><![CDATA[wx342fca817ff0f3f7]]></appid>  
             *    <mch_id><![CDATA[1232569002]]></mch_id>  
             *    <nonce_str><![CDATA[WJ3oEFjYdF4PXaOT]]></nonce_str>  
             *    <sign><![CDATA[508EA5DFB865509DF2CA51ED0065790C]]></sign>  
             *    <result_code><![CDATA[SUCCESS]]></result_code>  
             *    <prepay_id><![CDATA[wx20150325162732b7b9faa78b0136774558]]></prepay_id>  
             *    <trade_type><![CDATA[APP]]></trade_type> 
             *  </xml>
             */
            if(xn.SelectSingleNode("return_code")!= null && xn.SelectSingleNode("return_code").InnerText.ToUpper() == "SUCCESS"
                && xn.SelectSingleNode("result_code")!=null && xn.SelectSingleNode("result_code").InnerText.ToUpper() == "SUCCESS") 
            {
                string _package = "Sign=WXpay"; 
                string _prepay_id = string.Empty;
                _prepay_id = xn.SelectSingleNode("prepay_id").InnerText;

                #region 设置支付参数 输出页面  该部分参数请勿随意修改 ============== 
                var paySignReqHandler = new WechatRequest(httpContext);
                paySignReqHandler.setParameter("appid", WechatPayConfig.APPID);
                paySignReqHandler.setParameter("noncestr", _nonce_str);
                paySignReqHandler.setParameter("package", _package);
                paySignReqHandler.setParameter("partnerid", WechatPayConfig.MchID);
                paySignReqHandler.setParameter("prepayid", _prepay_id);
                paySignReqHandler.setParameter("timestamp", _time_stamp);
                string _pay_sign = paySignReqHandler.CreateMd5Sign("key", WechatPayConfig.ApiKey);
                #endregion

                WechatPayClientParamters _client_param = new WechatPayClientParamters()
                {
                    orderid = orderId,
                    appid = WechatPayConfig.APPID,
                    noncestr = _nonce_str,
                    package = _package,
                    partnerid = WechatPayConfig.MchID,
                    prepayid = _prepay_id,
                    timestamp = _time_stamp,
                    sign = _pay_sign,
                };
                return _client_param;
            }
            return null;
        }
    }
}