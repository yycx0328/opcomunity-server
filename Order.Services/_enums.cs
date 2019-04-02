using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Services
{
    public enum ChargeTypeConfig : int 
    {
        [Remark("Alipay")]
        AL,
        [Remark("Wechat Pay")]
        WC
    }

    public enum OrderStatusConfig : int
    {
        [Remark("下单")]
        Init,
        [Remark("成功")]
        Success,
        [Remark("失败")]
        Failure
    }

    public enum OrderCallbackStatusConfig : int
    {
        [Remark("初始化")]
        Init,
        [Remark("成功")]
        Success,
        [Remark("失败")]
        Failure
    }

    public enum OrderTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("订单签名失败")]
        OrderSignErr = 2000
    }
}
