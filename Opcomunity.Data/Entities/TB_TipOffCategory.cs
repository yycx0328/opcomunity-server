using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_TipOffCategory
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int SortId { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
