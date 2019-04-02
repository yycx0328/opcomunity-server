using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_AnchorCategoryRelation
    {
        public long AnchorId { get; set; }
        public int CategoryId { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
