using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_TopicComment
    {
        public long Id { get; set; }
        public long TopicId { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }
        public string Ip { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CommentTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
