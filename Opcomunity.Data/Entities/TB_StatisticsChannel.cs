using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_StatisticsChannel
    {
        public System.DateTime Date { get; set; }
        public int Channel { get; set; }
        public long RegistCount { get; set; }
        public long CoinCharge { get; set; }
        public long VipCharge { get; set; }
        public long TicketCharge { get; set; }
    }
}
