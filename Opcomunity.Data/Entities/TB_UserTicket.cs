using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserTicket
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Nullable<long> AnchorId { get; set; }
        public string Category { get; set; }
        public int Cost { get; set; }
        public Nullable<int> TotalTicket { get; set; }
        public Nullable<int> RemainderTicket { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
