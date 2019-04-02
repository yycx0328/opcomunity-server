using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_OrderAlipayCallbackResult
    {
        public long Id { get; set; }
        public string NotifyId { get; set; }
        public string AppId { get; set; }
        public string TradeNo { get; set; }
        public string OutTradeNo { get; set; }
        public System.DateTime NotifyTime { get; set; }
        public string NotifyType { get; set; }
        public string Charset { get; set; }
        public string Version { get; set; }
        public string SignType { get; set; }
        public string Sign { get; set; }
        public string BuyerId { get; set; }
        public string BuyerLogonId { get; set; }
        public string SellerId { get; set; }
        public string SellerEmail { get; set; }
        public string TradeStatus { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> ReceiptAmount { get; set; }
        public Nullable<decimal> BuyerPayAmount { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Nullable<System.DateTime> GmtCreate { get; set; }
        public Nullable<System.DateTime> GmtPayment { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
    }
}
