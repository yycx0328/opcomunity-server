using Opcomunity.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// 验证是否为登录用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsLoginUser(long userId, string token);

        /// <summary>
        /// 生成唯一订单号（订单号长度28位）
        /// </summary>
        /// <param name="suffix"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetUniqueOrderId(string suffix, long userId);
        
        /// <summary>
        /// 保存支付宝支付订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <param name="orderString"></param>
        /// <returns></returns>
        bool SaveAlipayOrder(long userId, string orderId, decimal amount,int coinCount, string orderString, string orderType);

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        bool SaveWechatPayOrder(long userId, string orderId, decimal amount,int totalCoin, string ticketType);
        
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        TB_OrderCharge GetChargeOrderById(string orderId);
         
        /// <summary>
        /// 充值成功处理
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="coinCount"></param>
        /// <returns></returns>
        bool ChargeCoin(long userId, int coinCount, string orderId);

        bool VipChargeCoin(long userId, string orderId, int totalMoney, string vipType);

        bool TicketChargeCoin(long userId, string orderId, int totalMoney, string vipType);

        TB_VIPConfig GetVipConfig(string key);

        TB_TicketConfig GetTicketConfig(string key);

        List<TB_VIPConfig> GetVipConfig();

        List<TB_TicketConfig> GetTicketConfig();

        /// <summary>
        /// 获取VIP用户的最晚到期时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DateTime GetUserVipMaxEndTime(long userId);

        /// <summary>
        /// 保存Alipay回调通知
        /// </summary>
        /// <param name="model"></param>
        void SaveAlipayCallbackResult(TB_OrderAlipayCallbackResult model); 

        /// <summary>
        /// 更新失败订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool UpdateFailureOrder(string orderId);

        /// <summary>
        /// 获取VIP结束时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="vipType"></param>
        /// <returns></returns>
        DateTime GetEndTime(DateTime startTime, string vipType);
    }
}
