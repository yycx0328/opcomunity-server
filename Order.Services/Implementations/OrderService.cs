using Opcomunity.Data.Entities;
using Order.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Services.Implementations
{
    public class OrderService : ServiceBase, IOrderService
    {
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

        public bool UpdateSuccessOrder(string orderId)
        {
            using (var context = base.NewContext())
            { 
                var order = context.TB_OrderCharge.FirstOrDefault(p=>p.OrderId == orderId && p.Status != (int)OrderStatusConfig.Success);
                if (null == order)
                    return false;
                order.Status = (int)OrderStatusConfig.Success;
                order.StatusDescription = OrderStatusConfig.Success.GetRemark();
                order.ChargeTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;

                var callback = context.TB_OrderAlipayCallbackResult.OrderByDescending(p => p.NotifyTime)
                    .FirstOrDefault(p => p.OutTradeNo == orderId && p.Status != (int)OrderCallbackStatusConfig.Success);
                if (null == callback)
                    return false;
                callback.Status = (int)OrderCallbackStatusConfig.Success;
                callback.StatusDescription = OrderCallbackStatusConfig.Success.GetRemark();
                return context.SaveChanges() > 0;
            }
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
    }
}
