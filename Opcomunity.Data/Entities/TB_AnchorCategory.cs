using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_AnchorCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortId { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
