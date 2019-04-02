using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserPhoto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int SortId { get; set; }
        public string ImageWebPath { get; set; }
        public string ThumbnailPath { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
