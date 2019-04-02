using Infrastructure;
using log4net;
using Mvc;
using Opcomunity.Models;
using Opcomunity.Services;
using Opcomunity.Services.Config;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebSite.Models;
using System.Linq;

namespace WebSite.Controllers
{
    public class ChargeController : JsonController
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        #region 充值余额
        public JsonWebResult AlipayTakeOrder()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                string str_amount = TypeHelper.TryParse(_requestParms.GetValue("amount"), "");
                decimal amount;
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(str_amount) || !decimal.TryParse(str_amount, out amount))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IOrderService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                string orderId = service.GetUniqueOrderId(ChargeTypeConfig.AL.ToString(), userId);
                Alipay alipay = new Alipay();
                string orderString = alipay.GetOrderString(orderId, amount, OrderTypeConfig.None.ToString());

                if (string.IsNullOrEmpty(orderString))
                {
                    json.state = (int)OrderTips.OrderSignErr;
                    json.message = OrderTips.OrderSignErr.GetRemark();
                    return ToJson(json);
                }

                int coinCount = (int)(amount * GoodsConfig.ExchargeRate);
                service.SaveAlipayOrder(userId, orderId, amount, coinCount, orderString, OrderTypeConfig.None.ToString());
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { OrderString = orderString };
                return ToJson(json);
            });
        }

        //public JsonWebResult WechatPayTakeOrder()
        //{
        //    return Try(() =>
        //    {
        //        JsonBase json = new JsonBase();
        //        Dictionary<string, string> _requestParms = new Dictionary<string, string>();
        //        ValidateTips _state = ValidateTips.Error_Init;

        //        if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
        //        {
        //            json.state = (int)_state;
        //            json.message = _state.GetRemark();
        //            return ToJson(json);
        //        }

        //        long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
        //        string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
        //        string str_amount = TypeHelper.TryParse(_requestParms.GetValue("amount"), "");
        //        decimal amount;
        //        if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(str_amount) || !decimal.TryParse(str_amount, out amount))
        //        {
        //            json.state = (int)ValidateTips.Error_BusinessParams;
        //            json.message = ValidateTips.Error_BusinessParams.GetRemark();
        //            return ToJson(json);
        //        }

        //        var service = Ioc.Get<IOrderService>();
        //        if (!service.IsLoginUser(userId, token))
        //        {
        //            json.state = (int)ValidateTips.Error_UserAccount;
        //            json.message = ValidateTips.Error_UserAccount.GetRemark();
        //            return ToJson(json);
        //        }

        //        string orderId = service.GetUniqueOrderId(ChargeTypeConfig.WX.ToString(), userId);
        //        WechatPay pay = new WechatPay();
        //        var clientParameters = pay.GetClientParamters(this.HttpContext, orderId, amount);

        //        if (clientParameters == null)
        //        {
        //            json.state = (int)OrderTips.WechatPrepayErr;
        //            json.message = OrderTips.WechatPrepayErr.GetRemark();
        //            return ToJson(json);
        //        }

        //        service.SaveWechatPayOrder(userId, orderId, amount);
        //        json.state = (int)ValidateTips.Success;
        //        json.message = ValidateTips.Success.GetRemark();
        //        json.data = new { ClientParameters = clientParameters };
        //        return ToJson(json);
        //    });
        //}

        public JsonWebResult WechatPayTakeOrder()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                string str_amount = TypeHelper.TryParse(_requestParms.GetValue("amount"), "");
                string os = TypeHelper.TryParse(_requestParms.GetValue("os"), "");
                decimal amount;
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(str_amount) || !decimal.TryParse(str_amount, out amount))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                int source = 1;
                if (os == "android")
                    source = 1;
                else
                    source = 2;

                //#region 测试专用
                //if (amount == 20)
                //    amount = 1; 
                //#endregion

                var service = Ioc.Get<IOrderService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                string orderId = service.GetUniqueOrderId(ThirdWxPayConfig.AppId + "_", userId);
                ThirdWxPay pay = new ThirdWxPay();
                var clientParameters = pay.GetPerOrderString(orderId, amount * 100, source, OrderTypeConfig.None.ToString());
                if (clientParameters == null)
                {
                    json.state = (int)OrderTips.WechatPrepayErr;
                    json.message = OrderTips.WechatPrepayErr.GetRemark();
                    return ToJson(json);
                }
                var totalCoin = (int)(amount * GoodsConfig.ExchargeRate);
                service.SaveWechatPayOrder(userId, orderId, amount, totalCoin, OrderTypeConfig.None.ToString());
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { ClientParameters = clientParameters };
                return ToJson(json);
            });
        }
        #endregion

        public JsonWebResult GetVipChargeMoneyConfig()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IOrderService>();
                var list = service.GetVipConfig();

                if (list == null || list.Count == 0)
                {
                    json.state = (int)ValidateTips.Success;
                    json.message = ValidateTips.Success.GetRemark();
                }
                else
                {

                    json.state = (int)ValidateTips.Success;
                    json.message = ValidateTips.Success.GetRemark();
                    json.data = from v in list
                                select new
                                {
                                    v.Key,
                                    v.Name,
                                    v.OriginalPrice,
                                    v.DiscountPrice,
                                    v.DonateCoin,
                                    v.IsRecommand,
                                    v.SortId
                                };
                }
                return ToJson(json);
            });
        }

        #region 充值VIP
        public JsonWebResult VipAlipayTakeOrder()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                string vipType = TypeHelper.TryParse(_requestParms.GetValue("viptype"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(vipType))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if (vipType != OrderTypeConfig.Month.ToString()
                    && vipType != OrderTypeConfig.Quarter.ToString()
                    && vipType != OrderTypeConfig.HalfYear.ToString()
                    && vipType != OrderTypeConfig.Year.ToString())
                {
                    json.state = 1001;
                    json.message = "VIP类型错误";
                    return ToJson(json);
                }

                var service = Ioc.Get<IOrderService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                string orderId = service.GetUniqueOrderId(ChargeTypeConfig.AL.ToString(), userId);
                // VIP金币数
                var vipConfig = service.GetVipConfig(vipType);
                if (vipConfig == null)
                {
                    json.state = 1003;
                    json.message = "参数错误";
                    return ToJson(json);
                }
                decimal amount = vipConfig.DiscountPrice;

                Alipay alipay = new Alipay();
                string orderString = alipay.GetOrderString(orderId, amount,vipType);

                if (string.IsNullOrEmpty(orderString))
                {
                    json.state = (int)OrderTips.OrderSignErr;
                    json.message = OrderTips.OrderSignErr.GetRemark();
                    return ToJson(json);
                }
                // VIP充值，不直接充钻石
                service.SaveAlipayOrder(userId, orderId, amount,0, orderString,vipType);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { OrderString = orderString };
                return ToJson(json);
            });
        }
        
        public JsonWebResult VipWechatPayTakeOrder()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                string vipType = TypeHelper.TryParse(_requestParms.GetValue("viptype"), "");
                string os = TypeHelper.TryParse(_requestParms.GetValue("os"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(vipType))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if (vipType != OrderTypeConfig.Month.ToString()
                && vipType != OrderTypeConfig.Quarter.ToString()
                && vipType != OrderTypeConfig.HalfYear.ToString()
                && vipType != OrderTypeConfig.Year.ToString())
                {
                    json.state = 1001;
                    json.message = "VIP类型错误";
                    return ToJson(json);
                }

                int source = 1;
                if (os == "android")
                    source = 1;
                else
                    source = 2;

                var service = Ioc.Get<IOrderService>();
                if (!service.IsLoginUser(userId, token))
                {
                    json.state = (int)ValidateTips.Error_UserAccount;
                    json.message = ValidateTips.Error_UserAccount.GetRemark();
                    return ToJson(json);
                }

                //var maxEndTime = service.GetUserVipMaxEndTime(userId);
                //DateTime startTime, endTime;
                //if (maxEndTime != null)
                //    startTime = maxEndTime;
                //else
                //    startTime = DateTime.Now;
                //endTime = service.GetEndTime(startTime, vipType);
                //var totalDay = (endTime - startTime).Days + 1;
                //var totalCoin = totalDay * TypeHelper.TryParse(ConfigHelper.GetValue("VIPPerDayCoin"), 0);

                // VIP金币数
                var vipConfig = service.GetVipConfig(vipType);
                if (vipConfig == null)
                {
                    json.state = 1003;
                    json.message = "参数错误";
                    return ToJson(json);
                }
                string orderId = service.GetUniqueOrderId(ThirdWxPayConfig.AppId + "_", userId);
                decimal amount = vipConfig.DiscountPrice;
                ThirdWxPay pay = new ThirdWxPay();
                var clientParameters = pay.GetPerOrderString(orderId, amount * 100, source, vipType);
                if (clientParameters == null)
                {
                    json.state = (int)OrderTips.WechatPrepayErr;
                    json.message = OrderTips.WechatPrepayErr.GetRemark();
                    return ToJson(json);
                }

                service.SaveWechatPayOrder(userId, orderId, amount, 0, vipType);
                json.state = (int)ValidateTips.Success;
                json.message = ValidateTips.Success.GetRemark();
                json.data = new { ClientParameters = clientParameters };
                return ToJson(json);
            });
        }
        #endregion

        public JsonWebResult GetTicketConfig()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                var service = Ioc.Get<IOrderService>();
                var list = service.GetTicketConfig();

                if (list == null || list.Count == 0)
                {
                    json.state = (int)ValidateTips.Success;
                    json.message = ValidateTips.Success.GetRemark();
                }
                else
                {

                    json.state = (int)ValidateTips.Success;
                    json.message = ValidateTips.Success.GetRemark();
                    json.data = from v in list
                                select new
                                {
                                    v.Key,
                                    v.Name,
                                    v.OriginalPrice,
                                    v.DiscountPrice,
                                    v.ChatCount,
                                    v.IsRecommand,
                                    v.SortId
                                };
                }
                return ToJson(json);
            });
        }

        #region 充值VIP
        public JsonWebResult TicketAlipayTakeOrder()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                string ticketType = TypeHelper.TryParse(_requestParms.GetValue("tickettype"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(ticketType))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if (ticketType == OrderTypeConfig.Limit.ToString() || ticketType==OrderTypeConfig.UnLimit.ToString())
                {
                    var service = Ioc.Get<IOrderService>();
                    if (!service.IsLoginUser(userId, token))
                    {
                        json.state = (int)ValidateTips.Error_UserAccount;
                        json.message = ValidateTips.Error_UserAccount.GetRemark();
                        return ToJson(json);
                    }

                    string orderId = service.GetUniqueOrderId(ChargeTypeConfig.AL.ToString(), userId);
                    // 获取邮票价格
                    var ticketConfig = service.GetTicketConfig(ticketType);
                    if (ticketConfig == null)
                    {
                        json.state = 1003;
                        json.message = "参数错误";
                        return ToJson(json);
                    }
                    decimal amount = ticketConfig.DiscountPrice;

                    Alipay alipay = new Alipay();
                    string orderString = alipay.GetOrderString(orderId, amount, ticketType);

                    if (string.IsNullOrEmpty(orderString))
                    {
                        json.state = (int)OrderTips.OrderSignErr;
                        json.message = OrderTips.OrderSignErr.GetRemark();
                        return ToJson(json);
                    }

                    service.SaveAlipayOrder(userId, orderId, amount, 0, orderString, ticketType);
                    json.state = (int)ValidateTips.Success;
                    json.message = ValidateTips.Success.GetRemark();
                    json.data = new { OrderString = orderString };
                    return ToJson(json);
                }
                else
                {
                    json.state = 1001;
                    json.message = "邮票类型错误";
                    return ToJson(json);
                }

            });
        }

        public JsonWebResult TicketWechatPayTakeOrder()
        {
            return Try(() =>
            {
                JsonBase json = new JsonBase();
                Dictionary<string, string> _requestParms = new Dictionary<string, string>();
                ValidateTips _state = ValidateTips.Error_Init;

                if (!HttpContext.CheckPostRequestParam(SECURITYKEY, out _requestParms, out _state))
                {
                    json.state = (int)_state;
                    json.message = _state.GetRemark();
                    return ToJson(json);
                }

                long userId = TypeHelper.TryParse(_requestParms.GetValue("userid"), 0L);
                string token = TypeHelper.TryParse(_requestParms.GetValue("token"), "");
                string ticketType = TypeHelper.TryParse(_requestParms.GetValue("tickettype"), "");
                string os = TypeHelper.TryParse(_requestParms.GetValue("os"), "");
                if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(ticketType))
                {
                    json.state = (int)ValidateTips.Error_BusinessParams;
                    json.message = ValidateTips.Error_BusinessParams.GetRemark();
                    return ToJson(json);
                }

                if (ticketType == OrderTypeConfig.Limit.ToString() || ticketType == OrderTypeConfig.UnLimit.ToString())
                {
                    int source = 1;
                    if (os == "android")
                        source = 1;
                    else
                        source = 2;

                    var service = Ioc.Get<IOrderService>();
                    if (!service.IsLoginUser(userId, token))
                    {
                        json.state = (int)ValidateTips.Error_UserAccount;
                        json.message = ValidateTips.Error_UserAccount.GetRemark();
                        return ToJson(json);
                    }

                    // VIP金币数
                    var ticketConfig = service.GetTicketConfig(ticketType);
                    if (ticketConfig == null)
                    {
                        json.state = 1003;
                        json.message = "参数错误";
                        return ToJson(json);
                    }
                    string orderId = service.GetUniqueOrderId(ThirdWxPayConfig.AppId + "_", userId);
                    decimal amount = ticketConfig.DiscountPrice;
                    ThirdWxPay pay = new ThirdWxPay();
                    var clientParameters = pay.GetPerOrderString(orderId, amount * 100, source, ticketType);
                    if (clientParameters == null)
                    {
                        json.state = (int)OrderTips.WechatPrepayErr;
                        json.message = OrderTips.WechatPrepayErr.GetRemark();
                        return ToJson(json);
                    }

                    service.SaveWechatPayOrder(userId, orderId, amount, 0, ticketType);
                    json.state = (int)ValidateTips.Success;
                    json.message = ValidateTips.Success.GetRemark();
                    json.data = new { ClientParameters = clientParameters };
                    return ToJson(json);
                }
                else
                {
                    json.state = 1001;
                    json.message = "邮票类型错误";
                    return ToJson(json);
                }
            });
        }
        #endregion
    }
}