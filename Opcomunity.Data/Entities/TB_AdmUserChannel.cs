using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_AdmUserChannel
    {
        public long Id { get; set; }
        public int AdmUserId { get; set; }
        public int ChannelId { get; set; }
        public int Deduction { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
