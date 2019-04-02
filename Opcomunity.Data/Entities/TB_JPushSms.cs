using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_JPushSms
    {
        public long Id { get; set; }
        public string PhoneNo { get; set; }
        public int TemplateId { get; set; }
        public string MessageId { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string Code { get; set; }
        public string Ip { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
