using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_Gift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Conver { get; set; }
        public int OriginalPrice { get; set; }
        public bool IsDiscount { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<System.DateTime> DiscountStart { get; set; }
        public Nullable<System.DateTime> DiscountEnd { get; set; }
        public int SortId { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
