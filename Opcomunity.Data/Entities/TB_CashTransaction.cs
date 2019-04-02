using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_CashTransaction
    {
        public string Id { get; set; }
        public long UserId { get; set; }
        public int CoinCount { get; set; }
        public int CashMoney { get; set; }
        public int CashRatio { get; set; }
        public string CashAccount { get; set; }
        public string CashName { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public System.DateTime CashTime { get; set; }
        public string Ip { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
