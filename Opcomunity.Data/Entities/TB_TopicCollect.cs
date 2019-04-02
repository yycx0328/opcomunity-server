using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_TopicCollect
    {
        public long UserId { get; set; }
        public long TopicId { get; set; }
        public System.DateTime CollectTime { get; set; }
    }
}
