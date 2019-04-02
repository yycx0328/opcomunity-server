using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class LoginUserModel
    {
        public Int64 UserId { get; set; }
        public string Token { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string PhoneNo { get; set; } 
        public string WeChat { get; set; } 
        public string QQ { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public DateTime? Birthday { get; set; }
        public string Constellation { get; set; }
        public long CurrentChargeCoin { get; set; }
        public long CurrentIncomeCoin { get; set; }
        public bool IsAnchor { get; set; }
        public int AuthStatus { get; set; }
        public long Glamour { get; set; }
        public int CallRatio { get; set; }
        public int CashRatio { get; set; }
        public string NeteaseAccId { get; set; }
        public string NeteaseToken { get; set; }
        public int NeteaseLoginStatus { get; set; }
        public int NeteaseChatStatus { get; set; }
    }
}
