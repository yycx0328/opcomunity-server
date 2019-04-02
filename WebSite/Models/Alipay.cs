using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Opcomunity.Services;

namespace Opcomunity.Models
{
    public class Alipay
    {
        /// <summary>
        /// 商户客户端调用并获取签名后的订单信息，并返回签名后的订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string GetOrderString(string orderId, decimal amount, string type)
        { 
            IAopClient client = new DefaultAopClient(AlipayConfig.ALIPAY_GATEWAY, AlipayConfig.APPID,
                AlipayConfig.APP_PRIVATE_KEY, "json", "1.0", "RSA2", AlipayConfig.ALIPAY_PUBLIC_KEY, AlipayConfig.CHARSET, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。
            //以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = GoodsConfig.BODY;
            model.Subject = string.Format("支付宝{0}", GoodsConfig.SUBJECT);
            model.OutTradeNo = orderId;
            model.TimeoutExpress = "30m";
            model.TotalAmount = amount.ToString();
            model.GoodsType = "0";
            model.ProductCode = "QUICK_MSECURITY_PAY";
            request.SetBizModel(model);
            if(type == OrderTypeConfig.None.ToString())
                request.SetNotifyUrl(AlipayConfig.NOTIFY_URL);
            else if(type == OrderTypeConfig.Limit.ToString() || type == OrderTypeConfig.UnLimit.ToString())
                request.SetNotifyUrl(AlipayConfig.TICKET_NOTIFY_URL);
            else
                request.SetNotifyUrl(AlipayConfig.VIP_NOTIFY_URL);
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            //Response.Write(HttpUtility.HtmlEncode(response.Body));
            //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
            return response.Body;
        }
    }
}