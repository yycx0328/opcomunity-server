using System;
using System.Collections.Generic;

namespace Opcomunity.Passport.Entities
{
    public partial class TB_UserAuth
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string IdentityType { get; set; }
        public string Identifier { get; set; }
        public string Credential { get; set; }
        public string Ip { get; set; }
        public string FirstLoginApp { get; set; }
        public bool IsLegal { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime LastLoginTime { get; set; }
    }
}
