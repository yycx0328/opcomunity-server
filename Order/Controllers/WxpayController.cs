using Infrastructure;
using log4net;
using Opcomunity.Data.Entities;
using Opcomunity.Services;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Order.Controllers
{
    public class WxpayController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        private static readonly object objlock = new object();

        // GET: Alipay
        public void TradePayCallBack()
        {
            lock (objlock)
            {
                Log4NetHelper.Info(log, "=======================Wechat TradePayCallBack Start=======================");
                this.HttpContext.Response.ContentType = "text/plain";
                bool success = DoProcess();
                string _reutn_code = success ? "SUCCESS" : "FAIL";
                string _return_msg = success ? "OK" : "ERROR";
                Dictionary<string, string> _dic = new Dictionary<string, string>();
                _dic.Add("return_code", _reutn_code);
                _dic.Add("return_msg", _return_msg);
                string _data = parseXML(_dic);
                Log4NetHelper.Info(log, "_data:" + _data);
                Log4NetHelper.Info(log, "=======================Wechat TradePayCallBack End=======================");
                this.HttpContext.Response.Write(_data);
            }
        }

        private bool DoProcess()
        {
            try
            {
                var service = Ioc.Get<IOrderService>();
                 
                string _msg = string.Empty;
                SortedDictionary<string, string> reqPara = new SortedDictionary<string, string>();

                WechatResponse _resp_handler = new WechatResponse(this.HttpContext);
                _resp_handler.setKey(WechatPayConfig.ApiKey);
                //SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查
                string return_code = _resp_handler.getParameter("return_code");

                if (return_code.ToUpper() != "SUCCESS" || !_resp_handler.isWXsign())
                    return false;

                #region 协议参数=====================================
                //--------------协议参数--------------------------------------------------------
                ////返回信息，如非空，为错误原因签名失败参数格式校验错误
                //string return_msg = _resp_handler.getParameter("return_msg");
                ////微信分配的公众账号 ID
                //string appid = _resp_handler.getParameter("appid");

                ////以下字段在 return_code 为 SUCCESS 的时候有返回--------------------------------
                ////微信支付分配的商户号
                //string mch_id = _resp_handler.getParameter("mch_id");
                ////微信支付分配的终端设备号
                //string device_info = _resp_handler.getParameter("device_info");
                ////微信分配的公众账号 ID
                //string nonce_str = _resp_handler.getParameter("nonce_str");
                //业务结果 SUCCESS/FAIL
                string result_code = _resp_handler.getParameter("result_code");
                //错误代码 
                string err_code = _resp_handler.getParameter("err_code");
                //结果信息描述
                string err_code_des = _resp_handler.getParameter("err_code_des");

                //以下字段在 return_code 和 result_code 都为 SUCCESS 的时候有返回---------------
                //-------------业务参数---------------------------------------------------------
                ////用户在商户 appid 下的唯一标识
                //string openid = _resp_handler.getParameter("openid");
                ////用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
                //string is_subscribe = _resp_handler.getParameter("is_subscribe");
                ////JSAPI、NATIVE、MICROPAY、APP
                //string trade_type = _resp_handler.getParameter("trade_type");
                ////银行类型，采用字符串类型的银行标识
                //string bank_type = _resp_handler.getParameter("bank_type");
                //订单总金额，单位为分
                int total_fee = TypeHelper.TryParse(_resp_handler.getParameter("total_fee"), 0);
                ////货币类型，符合 ISO 4217 标准的三位字母代码，默认人民币：CNY
                //string fee_type = _resp_handler.getParameter("fee_type");
                //微信支付订单号
                string transaction_id = _resp_handler.getParameter("transaction_id");
                //商户系统的订单号，与请求一致。
                string out_trade_no = _resp_handler.getParameter("out_trade_no");
                ////商家数据包，原样返回
                //string attach = _resp_handler.getParameter("attach");
                ////支 付 完 成 时 间 ， 格 式 为yyyyMMddhhmmss，如 2009 年12 月27日 9点 10分 10 秒表示为 20091227091010。时区为 GMT+8 beijing。该时间取自微信支付服务器
                //string time_end = _resp_handler.getParameter("time_end");
                if (out_trade_no.Equals("") || !return_code.Equals("SUCCESS") || !result_code.Equals("SUCCESS"))
                    return false;

                // ----------------------
                // 即时到帐处理业务开始
                // -----------------------
                TB_OrderCharge order = service.GetChargeOrderById(out_trade_no);
                if (null == order)
                    return false;

                //如果订单已经是支付成功的，则直接返回success
                if (order.Status == (int)OrderStatusConfig.Success)
                    return true;

                // 判断返回金额
                if (total_fee != 0 && (int)(order.ChargeMoney * 100) == total_fee)
                {
                    if (service.ChargeCoin(order.UserId, order.CoinCount, out_trade_no))
                        return true;
                    return false;
                }
                else
                {
                    bool result = service.UpdateFailureOrder(out_trade_no);
                    if (result) return true;
                    return false;
                } 
                // -----------------------
                // 即时到帐处理业务完毕
                // -----------------------
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
            } 
            return false;
        }
         
        /// <summary>
        /// 获取预支付 XML 参数组合
        /// </summary>
        /// <returns></returns>
        private string parseXML(Dictionary<string, string> _dic)
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (KeyValuePair<string, string> item in _dic)
            {
                if (Regex.IsMatch(item.Value, @"^[0-9.]$"))
                {
                    sb.Append("<" + item.Key + ">" + item.Value + "</" + item.Key + ">");
                }
                else
                {
                    sb.Append("<" + item.Key + "><![CDATA[" + item.Value + "]]></" + item.Key + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
}