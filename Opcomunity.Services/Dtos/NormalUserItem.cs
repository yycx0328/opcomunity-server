using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class NormalUserItem
    {
        public long UserId { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public string Description { get; set; }
        public bool IsVip { get; set; }
        public int? OnlineTime { get; set; }
    }
}
