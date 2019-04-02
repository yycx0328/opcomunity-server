using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_SendBatchMessage
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Message { get; set; }
        public int SendCount { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
