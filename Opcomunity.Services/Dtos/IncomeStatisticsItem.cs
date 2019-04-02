using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class IncomeStatisticsItem
    {
        public long CurrentIncome { get; set; }
        public long TotalIncome { get; set; }
        public long TotalLiveCallTime { get; set; }
        public int TotalGiftCount { get; set; }
        public int TotalInviteUser { get; set; }
    }
}
