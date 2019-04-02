using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_StatisticsCoin
    {
        public System.DateTime Date { get; set; }
        public long TotalRegistGive { get; set; }
        public long TotalCharge { get; set; }
        public long TotalActiveGive { get; set; }
        public long TotalSendGift { get; set; }
        public long TotalLiveCall { get; set; }
        public long TotalCash { get; set; }
        public long TotalCashMoney { get; set; }
        public long TotalInvite { get; set; }
        public long TotalIn { get; set; }
        public long TotalOut { get; set; }
        public long TotalRemaind { get; set; }
    }
}
