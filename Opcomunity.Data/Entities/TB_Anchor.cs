using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_Anchor
    {
        public long UserId { get; set; }
        public string Description { get; set; }
        public long Glamour { get; set; }
        public int CashRatio { get; set; }
        public int CallRatio { get; set; }
        public System.DateTime ApplyTime { get; set; }
        public bool IsAuth { get; set; }
        public Nullable<System.DateTime> AuthTime { get; set; }
        public bool IsBlack { get; set; }
    }
}
