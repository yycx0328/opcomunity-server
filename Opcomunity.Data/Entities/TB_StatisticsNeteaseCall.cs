using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_StatisticsNeteaseCall
    {
        public System.DateTime Date { get; set; }
        public long TotalDuration { get; set; }
        public long TotalFee { get; set; }
        public long TotalActualFee { get; set; }
    }
}
