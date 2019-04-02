using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class InviteRewardItem
    {
        public long? UserId { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public long TotalReward { get; set; }
    }
}
