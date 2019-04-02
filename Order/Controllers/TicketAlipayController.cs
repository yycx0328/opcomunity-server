using Aop.Api.Util;
using Infrastructure;
using log4net;
using Opcomunity.Data.Entities;
using Opcomunity.Services;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utility.Common;

namespace Order.Controllers
{
    public class TicketAlipayController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        // GET: Alipay
        public string TradePayCallBack()
        {
            try
            {
                Log4NetHelper.Info(log, "=======================Ticket TradePayCallBack Executing=======================");
                Dictionary<string, string> requstParams = HttpContext.GetRequestParms();
                var service = Ioc.Get<IOrderService>();

                if (requstParams != null && requstParams.Count > 0)
                {
                    //requstParams.ToList().ForEach((p) => { Log4NetHelper.Info(log, "Key/Value:" + p.Key + "     " + p.Value); });
                    #region 保存Alipay回调通知
                    TB_OrderAlipayCallbackResult callbackResultModel = new TB_OrderAlipayCallbackResult()
                    {
                        AppId = requstParams.GetValue("app_id"),
                        NotifyTime = TimeHelper.ConvertToDateTime(requstParams.GetValue("notify_time")),
                        NotifyType = requstParams.GetValue("notify_type"),
                        NotifyId = requstParams.GetValue("notify_id"),
                        Charset = TypeHelper.TryParse(requstParams.GetValue("charset"),"NA"),
                        Version = TypeHelper.TryParse(requstParams.GetValue("version"), "NA"),
                        Sign = requstParams.GetValue("sign"),
                        SignType = requstParams.GetValue("sign_type"),
                        TradeNo = requstParams.GetValue("trade_no"),
                        OutTradeNo = requstParams.GetValue("out_trade_no"),
                        BuyerId = requstParams.GetValue("buyer_id"),
                        BuyerLogonId = requstParams.GetValue("buyer_logon_id"),
                        BuyerPayAmount = TypeHelper.TryParse(requstParams.GetValue("buyer_pay_amount"), 0.0M),
                        SellerId = requstParams.GetValue("seller_id"),
                        SellerEmail = requstParams.GetValue("seller_email"),
                        TradeStatus = requstParams.GetValue("trade_status"),
                        TotalAmount = TypeHelper.TryParse(requstParams.GetValue("total_amount"), 0.0M),
                        ReceiptAmount = TypeHelper.TryParse(requstParams.GetValue("receipt_amount"), 0.0M),
                        Body = requstParams.GetValue("body"),
                        Subject = requstParams.GetValue("subject"),
                        GmtCreate = TimeHelper.ConvertToDateTime(requstParams.GetValue("gmt_create")),
                        GmtPayment = TimeHelper.ConvertToDateTime(requstParams.GetValue("gmt_payment")),
                        Status = (int)OrderCallbackStatusConfig.Init,
                        StatusDescription = OrderCallbackStatusConfig.Init.GetRemark()
                    };
                    service.SaveAlipayCallbackResult(callbackResultModel);
                    Log4NetHelper.Info(log, "Save AlipayCallbackResult Success");
                    #endregion


                    //切记alipaypublickey是支付宝的公钥，请去open.alipay.com对应应用下查看。
                    //bool RSACheckV1(IDictionary<string, string> parameters, string alipaypublicKey, string charset, string signType, bool keyFromFile)
                    bool signVerfied = AlipaySignature.RSACheckV1(requstParams, AlipayConfig.ALIPAY_PUBLIC_KEY,
                        AlipayConfig.CHARSET, "RSA2", false);
                    if (signVerfied)
                    {
                        Log4NetHelper.Info(log, "AlipaySignature True");
                        // TODO 验签成功后做以下判断
                        //1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号
                        string out_trade_no = requstParams.GetValue("out_trade_no");
                        if (string.IsNullOrEmpty(out_trade_no))
                            return "failure";

                        TB_OrderCharge order = service.GetChargeOrderById(out_trade_no);
                        if (null == order)
                            return "failure";

                        //2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额） 
                        string str_total_amount = requstParams.GetValue("total_amount");
                        decimal total_amount;
                        if (string.IsNullOrEmpty(str_total_amount) || !decimal.TryParse(str_total_amount, out total_amount))
                            return "failure";
                        if (total_amount != order.ChargeMoney)
                            return "failure";

                        //3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
                        string seller_id = TypeHelper.TryParse(requstParams.GetValue("seller_id"), "");
                        if (string.IsNullOrEmpty(seller_id) || seller_id != AlipayConfig.SELLER_ID)
                            return "failure";

                        //4、验证app_id是否为该商户本身
                        string app_id = TypeHelper.TryParse(requstParams.GetValue("app_id"), "");
                        if (string.IsNullOrEmpty(app_id) && app_id != AlipayConfig.APPID)
                            return "failure";

                        Log4NetHelper.Info(log, "Order Check  True");

                        //按照支付结果异步通知中的描述，对支付结果中的业务内容进行1\2\3\4二次校验，支付结果验证
                        //WAIT_BUYER_PAY 交易创建，等待买家付款                    （不触发通知）
                        //TRADE_CLOSED   未付款交易超时关闭，或支付完成后全额退款    （触发通知）
                        //TRADE_SUCCESS  交易支付成功                             （触发通知）
                        //TRADE_FINISHED 交易结束，不可退款                        （触发通知） 
                        string trade_status = TypeHelper.TryParse(requstParams.GetValue("trade_status"), "");
                        if (string.IsNullOrEmpty(trade_status))
                            return "failure";

                        //如果订单已经是支付成功的，则直接返回success
                        if (order.Status == (int)OrderStatusConfig.Success)
                            return "success";

                        //校验成功后在response中返回success，校验失败返回failure
                        if ((trade_status == "TRADE_SUCCESS" || trade_status == "TRADE_FINISHED"))
                        { 
                            if (service.TicketChargeCoin(order.UserId, order.OrderId, (int)total_amount, order.VipType))
                                return "success";
                            return "failure";
                        }
                        else
                        {
                            bool result = service.UpdateFailureOrder(out_trade_no);
                            if (result) return "success";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
            }

            return "failure";
        }
    }
}