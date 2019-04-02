using Opcomunity.Data.Entities;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using System;
using System.Linq;
using Utility.Common;
using System.Collections.Generic;

namespace Opcomunity.Services.Implementations
{
    public class OrderService : ServiceBase, IOrderService
    {
        private static int SqucenceID = 123456;

        /// <summary>
        /// 生成唯一订单号
        /// </summary>
        /// <param name="suffix"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUniqueOrderId(string suffix, long userId)
        {
            string strUserId = (userId % 88888888).ToString("00000000#");
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            SqucenceID++;
            string squcenceID = (SqucenceID % 999999).ToString("000000#");
            return string.Join("", suffix, strUserId, timeSpan, squcenceID);
        }

        /// <summary>
        /// 验证是否为登陆用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsLoginUser(long userId, string token)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                var user = query.SingleOrDefault();
                return user != null;
            }
        }

        /// <summary>
        /// 根据订单号获取充值订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public TB_OrderCharge GetChargeOrderById(string orderId)
        {
            using (var context = base.NewContext())
            {
                var query = from order in context.TB_OrderCharge
                            where order.OrderId == orderId
                            select order;
                return query.FirstOrDefault();
            }
        }

        public bool ChargeCoin(long userId, int coinCount, string orderId)
        {
            if (userId <= 0 || coinCount <= 0)
                return false;

            using (var context = base.NewContext())
            {
                Log4NetHelper.Info(log, "1、金币充值开始");
                #region 更新订单状态成功
                var order = context.TB_OrderCharge.FirstOrDefault(p => p.OrderId == orderId
                            && p.Status != (int)OrderStatusConfig.Success);
                if (null == order)
                    return false;
                order.Status = (int)OrderStatusConfig.Success;
                order.StatusDescription = OrderStatusConfig.Success.GetRemark();
                order.ChargeTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;
                #endregion

                Log4NetHelper.Info(log, "2、更新订单状态");

                #region 充值增加用户金币数
                var userInfo = context.TB_User.FirstOrDefault(p => p.Id == userId);
                if (userInfo == null)
                    return false;
                var userCoinModel = context.TB_UserCoin.SingleOrDefault(p => p.UserId == userId);
                if (userCoinModel != null)
                {
                    userCoinModel.CurrentCoin += coinCount;
                }
                else
                {
                    userCoinModel = new TB_UserCoin()
                    {
                        UserId = userId,
                        CurrentCoin = coinCount,
                        CurrentIncome = 0
                    };
                    context.TB_UserCoin.Add(userCoinModel);
                }

                Log4NetHelper.Info(log, "3、新增用户金币");

                long currentCount = userCoinModel.CurrentCoin;
                // 记录充值流水
                TB_UserCoinJournal userConinJournalModel = new TB_UserCoinJournal()
                {
                    UserId = userId,
                    CoinCount = coinCount,
                    CurrentCount = currentCount,
                    IOStatus = CoinIOStatusConfig.I.ToString(),
                    JournalType = (int)CoinJournalConfig.Charge,
                    JournalDesc = CoinJournalConfig.Charge.GetRemark(),
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_UserCoinJournal.Add(userConinJournalModel);
                #endregion

                Log4NetHelper.Info(log, "4、保存用户金币记录，并完成!");
                context.SaveChanges();
                return true;
            }
        }

        public bool VipChargeCoin(long userId, string orderId, int totalMoney, string vipType)
        {
            if (userId <= 0)
                return false;

            using (var context = base.NewContext())
            {
                Log4NetHelper.Info(log, "1、Vip充值开始");
                #region 更新订单状态成功
                var order = context.TB_OrderCharge.FirstOrDefault(p => p.OrderId == orderId
                            && p.Status != (int)OrderStatusConfig.Success);
                if (null == order)
                    return false;
                order.Status = (int)OrderStatusConfig.Success;
                order.StatusDescription = OrderStatusConfig.Success.GetRemark();
                order.ChargeTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;
                #endregion

                Log4NetHelper.Info(log, "2、更新订单状态");
                bool isRenew = false;// 用户是否续费标志
                DateTime startTime = DateTime.Now, endTime;
                var qVipUser = from v in context.TB_UserVIP
                            where v.UserId == userId
                            select v;
                if (qVipUser.Count() > 0)
                {
                    DateTime maxEndTime = qVipUser.Max(p => p.EndTime);
                    if (maxEndTime > DateTime.Now)
                    {
                        startTime = maxEndTime;
                        isRenew = true;
                    }
                }
                endTime = GetEndTime(startTime, vipType);
                int totalDay = (endTime - startTime).Days + 1;
                //int coinCount = TypeHelper.TryParse("VIPPerDayCoin",5);
                var totalCoinCount = (from v in context.TB_VIPConfig
                                 where v.Key == vipType && v.IsAvailable
                                 select v.DonateCoin).SingleOrDefault();
                if(totalCoinCount<=0)
                    return false;
                var coinCount = totalCoinCount / totalDay;

                Log4NetHelper.Info(log, "3、计算每日赠送金币数");
                // 如果用户当前是会员身份充值，则属于会员续费，本次充会员不赠送金币
                if (!isRenew) {
                    #region 充值增加用户金币数
                    var userInfo = context.TB_User.FirstOrDefault(p => p.Id == userId);
                    if (userInfo == null)
                        return false;
                    var userCoinModel = context.TB_UserCoin.SingleOrDefault(p => p.UserId == userId);
                    if (userCoinModel != null)
                    {
                        userCoinModel.CurrentCoin += coinCount;
                    }
                    else
                    {
                        userCoinModel = new TB_UserCoin()
                        {
                            UserId = userId,
                            CurrentCoin = coinCount,
                            CurrentIncome = 0
                        };
                        context.TB_UserCoin.Add(userCoinModel);
                    }

                    long currentCount = userCoinModel.CurrentCoin;
                    // 记录充值流水
                    TB_UserCoinJournal userConinJournalModel = new TB_UserCoinJournal()
                    {
                        UserId = userId,
                        CoinCount = coinCount,
                        CurrentCount = currentCount,
                        IOStatus = CoinIOStatusConfig.I.ToString(),
                        JournalType = (int)CoinJournalConfig.VIPPerDayDonate,
                        JournalDesc = CoinJournalConfig.VIPPerDayDonate.GetRemark(),
                        Ip = WebUtils.GetClientIP(),
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserCoinJournal.Add(userConinJournalModel);
                    #endregion
                }

                Log4NetHelper.Info(log, "4、第一天赠送金币");
                TB_UserVIP vipUser = new TB_UserVIP()
                {
                    UserId = userId,
                    CostMoney = totalMoney,
                    StartTime = startTime,
                    EndTime = endTime,
                    TotalCoin = totalCoinCount,
                    TotalDay = totalDay,
                    VIPType = vipType,
                    CreateTime = DateTime.Now
                };
                context.TB_UserVIP.Add(vipUser);
                Log4NetHelper.Info(log, "5、存储VIP用户，并完成");

                context.SaveChanges();
                return true;
            }
        }

        public DateTime GetEndTime(DateTime startTime, string vipType)
        {   
            if (vipType == OrderTypeConfig.Year.ToString())
                return startTime.AddYears(1);
            else if (vipType == OrderTypeConfig.HalfYear.ToString())
                return startTime.AddMonths(6);
            else if (vipType == OrderTypeConfig.Quarter.ToString())
                return startTime.AddMonths(3);
            else
                return startTime.AddMonths(1);
        }

        public bool UpdateFailureOrder(string orderId)
        {
            using (var context = base.NewContext())
            {
                var order = context.TB_OrderCharge.FirstOrDefault(p => p.OrderId == orderId && p.Status != (int)OrderStatusConfig.Success);
                if (null == order)
                    return false;
                order.Status = (int)OrderStatusConfig.Failure;
                order.StatusDescription = OrderStatusConfig.Failure.GetRemark();
                order.UpdateTime = DateTime.Now;
                return context.SaveChanges() > 0;
            }
        }

        public void SaveAlipayCallbackResult(TB_OrderAlipayCallbackResult model)
        {
            using (var context = base.NewContext())
            {
                context.TB_OrderAlipayCallbackResult.Add(model);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 保存支付宝支付订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <param name="orderString"></param>
        /// <returns></returns>
        public bool SaveAlipayOrder(long userId, string orderId, decimal amount,int coinCount, string orderString, string orderType)
        {
            using (var context = base.NewContext())
            {
                #region 保存签名的订单信息
                TB_OrderAlipayString model = new TB_OrderAlipayString()
                {
                    OrderId = orderId,
                    OrderString = orderString
                };
                context.TB_OrderAlipayString.Add(model);
                #endregion

                #region 生成订单
                TB_OrderCharge order = new TB_OrderCharge()
                {
                    OrderId = orderId,
                    UserId = userId,
                    AppId = AlipayConfig.APPID,
                    SellerId = AlipayConfig.SELLER_ID,
                    ChargeType = ChargeTypeConfig.AL.ToString(),
                    ChargeMoney = amount,
                    ExchargeRate = GoodsConfig.ExchargeRate,
                    CoinCount = coinCount,
                    Ip = WebUtils.GetClientIP(),
                    Subject = string.Format("支付宝{0}", GoodsConfig.SUBJECT),
                    Body = GoodsConfig.BODY,
                    VipType = orderType,
                    Status = (int)OrderStatusConfig.Init,
                    StatusDescription = OrderStatusConfig.Init.GetRemark(),
                    TakeOrderTime = DateTime.Now
                };
                context.TB_OrderCharge.Add(order);
                #endregion
                return context.SaveChanges() > 0;
            }
        }

        public bool SaveWechatPayOrder(long userId, string orderId, decimal amount,int totalCoin, string vipType)
        {
            using (var context = base.NewContext())
            {
                #region 生成订单
                TB_OrderCharge order = new TB_OrderCharge()
                {
                    OrderId = orderId,
                    UserId = userId,
                    AppId = WechatPayConfig.APPID,
                    SellerId = WechatPayConfig.MchID,
                    ChargeType = ChargeTypeConfig.WX.ToString(),
                    ChargeMoney = amount,
                    ExchargeRate = GoodsConfig.ExchargeRate,
                    CoinCount = totalCoin,
                    Ip = WebUtils.GetClientIP(),
                    Subject = string.Format("微信{0}", GoodsConfig.SUBJECT),
                    Body = GoodsConfig.BODY,
                    VipType = vipType,
                    Status = (int)OrderStatusConfig.Init,
                    StatusDescription = OrderStatusConfig.Init.GetRemark(),
                    TakeOrderTime = DateTime.Now
                };
                context.TB_OrderCharge.Add(order);
                #endregion
                return context.SaveChanges() > 0;
            }
        }

        public DateTime GetUserVipMaxEndTime(long userId)
        {
            using (var context = base.NewContext())
            {
                return context.TB_UserVIP.Where(p=>p.UserId == userId).Max(p=>p.EndTime);
            }
        }

        public List<TB_VIPConfig> GetVipConfig()
        {
            using (var context = base.NewContext())
            {
                var query = from v in context.TB_VIPConfig
                            where v.IsAvailable
                            orderby v.SortId
                            select v;
                return query.ToList();
            }
        }

        public TB_VIPConfig GetVipConfig(string key)
        {
            using (var context = base.NewContext())
            {
                var query = from v in context.TB_VIPConfig
                            where v.Key == key && v.IsAvailable
                            select v;
                return query.SingleOrDefault();
            }
        }

        public TB_TicketConfig GetTicketConfig(string key)
        {
            using (var context = base.NewContext())
            {
                var query = from v in context.TB_TicketConfig
                            where v.Key == key && v.IsAvailable
                            select v;
                return query.SingleOrDefault();
            }
        }

        public bool TicketChargeCoin(long userId, string orderId, int totalMoney, string ticketType)
        {
            if (userId <= 0)
                return false;

            using (var context = base.NewContext())
            {
                Log4NetHelper.Info(log, "1、邮票充值开始");
                #region 更新订单状态成功
                var order = context.TB_OrderCharge.FirstOrDefault(p => p.OrderId == orderId
                            && p.Status != (int)OrderStatusConfig.Success);
                if (null == order)
                    return false;
                order.Status = (int)OrderStatusConfig.Success;
                order.StatusDescription = OrderStatusConfig.Success.GetRemark();
                order.ChargeTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;
                #endregion

                Log4NetHelper.Info(log, "2、更新订单状态");

                var queryTicket = from v in context.TB_TicketConfig
                                       where v.Key == ticketType && v.IsAvailable
                                       select v;
                var ticket = queryTicket.SingleOrDefault();
                
                TB_UserTicket ticjetUser = new TB_UserTicket()
                {
                    UserId = userId,
                    AnchorId = null,
                    Category = ticketType,
                    Cost = totalMoney,
                    TotalTicket = ticket.ChatCount,
                    RemainderTicket = ticket.ChatCount,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                context.TB_UserTicket.Add(ticjetUser);
                Log4NetHelper.Info(log, "3、保存用户邮票，并完成");

                context.SaveChanges();
                return true;
            }
        }

        public List<TB_TicketConfig> GetTicketConfig()
        {
            using (var context = base.NewContext())
            {
                var query = from v in context.TB_TicketConfig
                            where v.IsAvailable
                            orderby v.SortId
                            select v;
                return query.ToList();
            }
        }
    }
}