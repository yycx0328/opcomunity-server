using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_Config
    {
        public string KeyId { get; set; }
        public string Value { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
