using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_NeteaseUserMessageRelation
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int MessageId { get; set; }
        public System.DateTime FinishTime { get; set; }
    }
}
