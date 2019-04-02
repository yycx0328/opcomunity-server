using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_NeteaseMsgConfig
    {
        public long Id { get; set; }
        public long AnchorId { get; set; }
        public int SortId { get; set; }
        public int Seconds { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public string Body { get; set; }
        public bool IsAvaiable { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
