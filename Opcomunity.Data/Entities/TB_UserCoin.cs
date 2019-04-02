using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserCoin
    {
        public long UserId { get; set; }
        public long CurrentCoin { get; set; }
        public long CurrentIncome { get; set; }
    }
}
