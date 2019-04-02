using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_NeteaseText
    {
        public long Id { get; set; }
        public int EventType { get; set; }
        public string MsgType { get; set; }
        public string FromNick { get; set; }
        public string MsgIdServer { get; set; }
        public string FromAccount { get; set; }
        public long FromUserId { get; set; }
        public string ToAccount { get; set; }
        public long ToUserId { get; set; }
        public string FromClientType { get; set; }
        public string FromDeviceId { get; set; }
        public string Body { get; set; }
        public string ConvType { get; set; }
        public string MsgIdClient { get; set; }
        public bool ResendFlag { get; set; }
        public long MsgTimestamp { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
