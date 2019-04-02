using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_Message
    {
        public long Id { get; set; }
        public Nullable<long> UserId { get; set; }
        public int Category { get; set; }
        public string CategoryDescription { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Parameters { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int SortId { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
