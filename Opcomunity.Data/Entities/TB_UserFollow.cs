using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserFollow
    {
        public long UserId { get; set; }
        public long FollowedUserId { get; set; }
        public System.DateTime FollowTime { get; set; }
    }
}
