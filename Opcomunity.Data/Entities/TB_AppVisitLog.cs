using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_AppVisitLog
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int Channel { get; set; }
        public string Version { get; set; }
        public string OS { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
