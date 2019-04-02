using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class OthersDetailInfo
    {
        public Int64 OthersUserId { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public string Description { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public DateTime? Birthday { get; set; }
        public string Constellation { get; set; }
        public long Glamour { get; set; }
        public string NeteaseAccId { get; set; }
        public int NeteaseLoginStatus { get; set; }
        public int NeteaseChatStatus { get; set; }
        public virtual IQueryable<UserPhotoItem> UserPhotoItems { get; set; }
    }
}
