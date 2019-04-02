using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{ 
    public sealed partial class TopicObjectItem
    {
        public long TopicId { get; set; }
        public string Bucket { get; set; }
        public string OssKey { get; set; }
        public string HashValue { get; set; }
        public long FileSize { get; set; }
        public string MimeType { get; set; }
        public string Ext { get; set; }
        public int SortId { get; set; }
        public bool IsLock { get; set; }
    }

}
