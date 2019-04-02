using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class AnchorItem
    {
        public long AnchorId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public string Description { get; set; }
        public long Glamour { get; set; }
        public int CallRatio { get; set; }
        public int AuthStatus { get; set; }
        public DateTime? AuthTime { get; set; }
        public string NeteaseAccId { get; set; }
        //public string NeteaseToken { get; set; }
        public int NeteaseChatStatus { get; set; }
    }
}
