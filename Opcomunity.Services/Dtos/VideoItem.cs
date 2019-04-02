using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class VideoItem
    {
        public long VideoId { get; set; }
        public long AnchorId { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public bool IsAnchor { get; set; }
        public int AuthStatus { get; set; }
        public int CallRatio { get; set; }
        public string NeteaseAccId { get; set; }
        public int NeteaseChatStatus { get; set; }
        public string Link { get; set; }
        public string ImgPath { get; set; }
        public int Praises { get; set; }
        public int Views { get; set; }
        public string Description { get; set; }
        public bool IsPraise { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
