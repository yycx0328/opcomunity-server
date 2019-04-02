using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_FeedBack
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public System.DateTime FeedBackTime { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Ip { get; set; }
    }
}
