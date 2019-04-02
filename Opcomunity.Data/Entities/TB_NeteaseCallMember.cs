using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_NeteaseCallMember
    {
        public long ChannelId { get; set; }
        public string AccId { get; set; }
        public int Duration { get; set; }
        public bool IsCaller { get; set; }
    }
}
