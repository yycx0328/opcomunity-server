using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_AnchorIdentity
    {
        public long UserId { get; set; }
        public string IdentityPositive { get; set; }
        public string IdentityOpposite { get; set; }
        public string Remark { get; set; }
    }
}
