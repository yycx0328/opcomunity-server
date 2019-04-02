using System;
using System.Collections.Generic;

namespace Opcomunity.Passport.Entities
{
    public partial class TB_User
    {
        public long Id { get; set; }
        public string RegistApplication { get; set; }
        public bool IsLegal { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
