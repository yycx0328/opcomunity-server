using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class FollowUserItem
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public string Description { get; set; }
        public bool IsAnchor { get; set; }
        public long Glamour { get; set; }
        public int NeteaseChatStatus { get; set; }
        public bool IsVip { get; set; }
        public DateTime FollowTime { get; set; }
        public virtual IQueryable<UserPhotoItem> UserPhotoItems { get; set; }
    }
}
