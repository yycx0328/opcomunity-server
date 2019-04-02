using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserTokenInfo
    {
        public long UserId { get; set; }
        public string UserToken { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime ExpireTime { get; set; }
    }
}
