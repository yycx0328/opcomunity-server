using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserInvite
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string PhoneNo { get; set; }
        public Nullable<long> NewUserId { get; set; }
        public int CostAwardRate { get; set; }
        public int CashoutAwardRate { get; set; }
        public string Ip { get; set; }
        public System.DateTime InviteTime { get; set; }
    }
}
