using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserTopicPayment
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TopicId { get; set; }
        public int OriginalPrice { get; set; }
        public int ActualPrice { get; set; }
        public bool IsDiscount { get; set; }
        public Nullable<int> Discount { get; set; }
        public string Ip { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
