using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_TipOff
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long AnchorId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Ip { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
