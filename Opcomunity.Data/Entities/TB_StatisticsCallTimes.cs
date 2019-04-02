using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_StatisticsCallTimes
    {
        public System.DateTime Date { get; set; }
        public long TotalTimes { get; set; }
        public long TotalSuccessTimes { get; set; }
        public long TotalFaildTimes { get; set; }
    }
}
