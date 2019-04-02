using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_StatisticsCharge
    {
        public System.DateTime Date { get; set; }
        public long TotalCharge { get; set; }
        public long ALCharge { get; set; }
        public long WXCharge { get; set; }
    }
}
