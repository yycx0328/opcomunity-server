using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_OrderWechatPayClientPatameter
    {
        public long Id { get; set; }
        public string OrderId { get; set; }
        public string AppId { get; set; }
        public string Noncestr { get; set; }
        public string PatnerId { get; set; }
        public string PrepayId { get; set; }
        public string TimeStamp { get; set; }
        public string Sign { get; set; }
    }
}
