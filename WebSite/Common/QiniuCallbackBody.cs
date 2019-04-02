using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite
{
    public class QiniuCallbackBody
    {
        public string Key { get; set; }
        public string HashValue { get; set; }
        public long FileSize { get; set; }
        public string Bucket { get; set; }
        public string MimeType { get; set; }
        public string Ext { get; set; }
        public long TopicId { get; set; }
        public int SortId { get; set; }
        public bool IsLock { get; set; }

    }
}