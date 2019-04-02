using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserVideoPraise
    {
        public long UserId { get; set; }
        public long VideoId { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
