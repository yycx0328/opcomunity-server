using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_Topic
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public long Views { get; set; }
        public long Collects { get; set; }
        public long Comments { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
