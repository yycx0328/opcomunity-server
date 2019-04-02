using Infrastructure;
using log4net;
using Opcomunity.Data.Entities;
using Opcomunity.Services;
using Opcomunity.Services.Config;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Order.Controllers
{
    public class TicketWxPayController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        private static readonly object objlock = new object();

        // GET: Alipay
        public void TradePayCallBack()
        {
            lock (objlock)
            {
                Log4NetHelper.Info(log, "=======================Ticket Wechat TradePayCallBack Start=======================");
                this.HttpContext.Response.ContentType = "text/plain";
                var result = DoProcess();
                var text = string.Empty;
                if (result)
                    text = "success";
                else
                    text = "invalid sign";
                Log4NetHelper.Info(log, "=======================Ticket Wechat TradePayCallBack End=======================");
                this.HttpContext.Response.Write(text);
            }
        }

        private bool DoProcess()
        {
            try
            {
                #region 获取参数
                String appkey = ThirdWxPayConfig.AppKey;
                Dictionary<string, string> requstParams = HttpContext.GetRequestParms();
                String appid = requstParams.GetValue("appid");
                String amount = requstParams.GetValue("amount");
                String itemname = requstParams.GetValue("itemname");
                String ordersn = requstParams.GetValue("ordersn");
                String orderdesc = requstParams.GetValue("orderdesc");
                String serialno = requstParams.GetValue("serialno");
                String sign = requstParams.GetValue("sign");
                Log4NetHelper.Info(log, "sign:" + sign);
                #endregion

                #region 拼接加密串
                String[] param = new String[6];
                param[0] = "appid=" + appid;
                param[1] = "amount=" + amount;
                param[2] = "itemname=" + itemname;
                param[3] = "ordersn=" + ordersn;
                param[4] = "orderdesc=" + orderdesc;
                param[5] = "serialno=" + serialno;
                Array.Sort(param);
                String signStr = "";
                bool flag = false;
                for (int i = 0; i < param.Length; i++)
                {
                    Console.WriteLine(param[i] + "  ");

                    if (!"".Equals(param[i]))
                    {
                        if (!flag)
                        {
                            signStr = param[i].Split('=')[1];
                            flag = true;
                        }
                        else
                        {
                            signStr += "|" + param[i].Split('=')[1];
                        }
                    }
                }
                if (signStr != "")
                {
                    signStr = signStr + "|" + appkey;
                }
                Log4NetHelper.Info(log, "signStr:" + signStr);
                 
                String md5Str = WebUtils.MD5(signStr, "UTF-8").ToLower();
                Log4NetHelper.Info(log, "md5Str:" + md5Str);
                #endregion

                #region 验证签名，并处理订单
                if (md5Str.Equals(sign))
                {
                    var service = Ioc.Get<IOrderService>();
                    // ----------------------
                    // 即时到帐处理业务开始
                    // -----------------------
                    TB_OrderCharge order = service.GetChargeOrderById(ordersn);
                    if (null == order)
                        return false;

                    //如果订单已经是支付成功的，则直接返回success
                    if (order.Status == (int)OrderStatusConfig.Success)
                        return true;

                    // 判断返回金额
                    var total_fee = TypeHelper.TryParse(amount, 0);
                    if (total_fee != 0 && (int)(order.ChargeMoney * 100) == total_fee)
                    {
                        if (service.TicketChargeCoin(order.UserId, ordersn, (int)order.ChargeMoney, order.VipType))
                            return true;
                        return false;
                    }
                    else
                    {
                        bool result = service.UpdateFailureOrder(ordersn);
                        if (result) return true;
                        return false;
                    }
                    // ----------------------
                    // 即时到帐处理业务结束
                    // ----------------------- 
                }
                else
                {
                    return false;
                } 
                #endregion 
            }
            catch (Exception ex)
            {
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
            }
            return false;
        }
    }
}