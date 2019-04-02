using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class LoginUserDetailInfoItem: UserDetailInfoItem
    {
        public string PhoneNo { get; set; }
        public string WeChat { get; set; }
        public string QQ { get; set; }
        public string Token { get; set; }
        public long CurrentCoin { get; set; }
        public long CurrentIncome { get; set; }
        //public bool IsAnchor { get; set; }
        //public int AuthStatus { get; set; }
        //public int CallRatio { get; set; }
        public int CashRatio { get; set; }
        public long VipExpireTime { get; set; }
        public int FollowedCount { get; set; }
        public string NeteaseToken { get; set; } 
    }
}
