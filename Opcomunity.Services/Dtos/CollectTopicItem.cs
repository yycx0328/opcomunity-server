using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class CollectTopicItem
    { 
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public int CollectCount { get; set; }
        public string TopicDescription { get; set; }
        public decimal TopicPrice { get; set; }
        public DateTime CollectTime { get; set; }
        public virtual IQueryable<TopicObjectItem> TopicItems { get; set; }
        public virtual IQueryable<TagItem> TagItems { get; set; }
    }
}
