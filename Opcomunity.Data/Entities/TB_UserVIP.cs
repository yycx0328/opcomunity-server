using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserVIP
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string VIPType { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int CostMoney { get; set; }
        public int TotalCoin { get; set; }
        public int TotalDay { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
