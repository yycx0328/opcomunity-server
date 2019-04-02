using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class LiveCallHistoryItem
    { 
        //public long Id { get; set; }
        public bool IsUnConnected { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string ThumbnailAvatar { get; set; }
        public bool IsAnchor { get; set; }
        public long CallTime { get; set; }
        public int CallRatio { get; set; }
        public int TotalFee { get; set; }
        public int TotalSecond { get; set; }
    }

    public class LiveCallHistoryItemReturn : LiveCallHistoryItem
    {
        public bool IsVip { get; set; }
    }
}
