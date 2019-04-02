using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_GiftTransaction
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long AnchorId { get; set; }
        public int GiftId { get; set; }
        public int OriginalPrice { get; set; }
        public int CostPrice { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Ip { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
