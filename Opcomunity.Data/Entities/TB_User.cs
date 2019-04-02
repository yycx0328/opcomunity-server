using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_User
    {
        public long Id { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public string PhoneNo { get; set; }
        public string WeChat { get; set; }
        public string QQ { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> Weight { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Constellation { get; set; }
        public string AlipayAccount { get; set; }
        public string Description { get; set; }
    }
}
