using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserCoinJournal
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CoinCount { get; set; }
        public long CurrentCount { get; set; }
        public string IOStatus { get; set; }
        public int JournalType { get; set; }
        public string JournalDesc { get; set; }
        public string Ip { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
