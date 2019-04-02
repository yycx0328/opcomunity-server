using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class TopicItem
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserAvatar { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string ThumbnailAvatar { get; set; }

        /// <summary>
        /// 话题Id
        /// </summary>
        public long TopicId { get; set; }

        /// <summary>
        /// 话题描述
        /// </summary>
        public string TopicDescription { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal TopicPrice { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime TopicDateTime { get; set; }

        /// <summary>
        /// 是否上锁
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        /// 阅览数
        /// </summary>
        public long ViewCount { get; set; }

        /// <summary>
        /// 收藏总数
        /// </summary>
        public long CollectCount { get; set; }

        /// <summary>
        /// 评论总数
        /// </summary>
        public long CommentCount { get; set; }

        /// <summary>
        /// 文件列表
        /// </summary>
        public virtual IQueryable<TopicObjectItem> TopicItems { get; set; }

        /// <summary>
        /// 标签列表
        /// </summary>
        public virtual IQueryable<TagItem> TagItems { get; set; }
    }
}
