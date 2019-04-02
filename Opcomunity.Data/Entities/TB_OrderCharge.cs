using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_OrderCharge
    {
        public string OrderId { get; set; }
        public long UserId { get; set; }
        public string AppId { get; set; }
        public string SellerId { get; set; }
        public string ChargeType { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public decimal ChargeMoney { get; set; }
        public int ExchargeRate { get; set; }
        public int CoinCount { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Ip { get; set; }
        public System.DateTime TakeOrderTime { get; set; }
        public Nullable<System.DateTime> ChargeTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string VipType { get; set; }
    }
}
