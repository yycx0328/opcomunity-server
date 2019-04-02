using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_QiniuUploadToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public System.DateTime ExpiresTime { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
