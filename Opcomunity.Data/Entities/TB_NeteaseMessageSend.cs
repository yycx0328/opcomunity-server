using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_NeteaseMessageSend
    {
        public long Id { get; set; }
        public long FromUserId { get; set; }
        public string FromAccId { get; set; }
        public long ToUserId { get; set; }
        public string ToAccId { get; set; }
        public int Type { get; set; }
        public string Body { get; set; }
        public System.DateTime WaitingSendTime { get; set; }
    }
}
