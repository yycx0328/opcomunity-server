using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_VIPConfig
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int OriginalPrice { get; set; }
        public int DiscountPrice { get; set; }
        public int DonateCoin { get; set; }
        public bool IsRecommand { get; set; }
        public int SortId { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
