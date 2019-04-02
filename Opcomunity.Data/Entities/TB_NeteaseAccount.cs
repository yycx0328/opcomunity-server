using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_NeteaseAccount
    {
        public long UserId { get; set; }
        public string NeteaseAccId { get; set; }
        public string NeteaseToken { get; set; }
        public bool IsAvailable { get; set; }
        public int LoginStatus { get; set; }
        public int ChatStatus { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
