using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_CallAnchor
    {
        public long Id { get; set; }
        public long CallId { get; set; }
        public long UserId { get; set; }
        public long AnchorId { get; set; }
        public int Sender { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int CallRatio { get; set; }
        public int TotalFee { get; set; }
        public string Ip { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
