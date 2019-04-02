using System;
using System.Collections.Generic;

namespace Opcomunity.Passport.Entities
{
    public partial class TB_ApplicationInfo
    {
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationKey { get; set; }
        public string SecurityIps { get; set; }
        public string Status { get; set; }
    }
}
