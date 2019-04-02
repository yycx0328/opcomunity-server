using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_OssObject
    {
        public long Id { get; set; }
        public long TopicId { get; set; }
        public string Bucket { get; set; }
        public string OssKey { get; set; }
        public string HashValue { get; set; }
        public long FileSize { get; set; }
        public string MimeType { get; set; }
        public string Ext { get; set; }
        public int SortId { get; set; }
        public bool IsLock { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
