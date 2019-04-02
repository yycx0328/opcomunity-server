using Opcomunity.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Services.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// 保存Alipay回调通知
        /// </summary>
        /// <param name="model"></param>
        void SaveAlipayCallbackResult(TB_OrderAlipayCallbackResult model);

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        TB_OrderCharge GetChargeOrderById(string orderId);

        /// <summary>
        /// 更新成功订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool UpdateSuccessOrder(string orderId);

        /// <summary>
        /// 更新失败订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool UpdateFailureOrder(string orderId); 
    }
}
