using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class DevoteRankItem
    {
        public bool IsAnchor { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public int TotalDevote { get; set; }
        public bool IsVip { get; set; }
    }
}
