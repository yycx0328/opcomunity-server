using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_StatisticsCash
    {
        public System.DateTime Date { get; set; }
        public long TotalCoinCount { get; set; }
        public long TotalCashMoney { get; set; }
    }
}
