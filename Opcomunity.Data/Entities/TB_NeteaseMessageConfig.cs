using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_NeteaseMessageConfig
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public string Body { get; set; }
        public bool IsAvaiable { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
