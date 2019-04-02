using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_NeteaseCall
    {
        public long ChannelId { get; set; }
        public long CallerId { get; set; }
        public long AnchorId { get; set; }
        public int TotalDuration { get; set; }
        public int Duration { get; set; }
        public int CallRatio { get; set; }
        public int TotalFee { get; set; }
        public int ActualTransferFee { get; set; }
        public string Ext { get; set; }
        public long CallTime { get; set; }
        public int EventType { get; set; }
        public string Type { get; set; }
        public int Live { get; set; }
        public string CallStatus { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
